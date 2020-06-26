using septa.Auth.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;

namespace septa.Auth.Domain.Repository
{
    public abstract class RepositoryBase<TEntity> : BasicRepositoryBase<TEntity>, IRepository<TEntity>
    where TEntity : class
    {

        public virtual Type ElementType => GetQueryable().ElementType;

        public virtual Expression Expression => GetQueryable().Expression;

        public virtual IQueryProvider Provider => GetQueryable().Provider;

        public virtual IQueryable<TEntity> WithDetails()
        {
            return GetQueryable();
        }

        public virtual IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetQueryable();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }

        protected abstract IQueryable<TEntity> GetQueryable();

        public abstract Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(predicate, includeDetails, cancellationToken);

            if (entity == null)
            {
                throw new Exception("Not Found Entity");
            }

            return entity;
        }

        public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);

        protected virtual TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
            where TQueryable : IQueryable<TEntity>
        {
            //if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<ISoftDelete>(), e => ((ISoftDelete)e).IsDeleted == false);
            //}

            //if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            //{
            //    var tenantId = CurrentTenant.Id;
            //    query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<IMultiTenant>(), e => ((IMultiTenant)e).TenantId == tenantId);
            //}

            return query;
        }
    }
}
