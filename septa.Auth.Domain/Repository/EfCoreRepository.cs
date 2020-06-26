using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using septa.Auth.Domain.Hellper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Interface.Entity;

namespace septa.Auth.Domain.Repository
{
    public class EfCoreRepository<TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>
    where TEntity : class
    {
        public virtual DbSet<TEntity> DbSet { get; set; }

        public readonly IUnitOfWork _unitOfWork;

        public EfCoreRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            DbSet = unitOfWork.Set<TEntity>();
        }


        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                await _unitOfWork.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Attach(entity);

            var updatedEntity = DbSet.Update(entity).Entity;

            if (autoSave)
            {
                await _unitOfWork.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return updatedEntity;
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await _unitOfWork.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails().ToListAsync(GetCancellationToken(cancellationToken))
                : await DbSet.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public override async Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails()
                    .Where(predicate)
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken))
                : await DbSet
                    .Where(predicate)
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable()
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }

            if (autoSave)
            {
                await _unitOfWork.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await _unitOfWork
                .Entry(entity)
                .Collection(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await _unitOfWork
                .Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<TEntity> WithDetails()
        {
            return base.WithDetails();

        }

        public override IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }
    }

    public class EfCoreRepository<TEntity, TKey> : EfCoreRepository<TEntity>, IEfCoreRepository<TEntity, TKey> where TEntity : class , IEntity<TKey>
    {
        public EfCoreRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));

            if (entity == null)
            {
                throw new Exception("Not Found Entity");
            }

            return entity;
        }

        public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails().FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken))
                : await DbSet.FindAsync(new object[] { id }, GetCancellationToken(cancellationToken));
        }

        public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }


    }
}
