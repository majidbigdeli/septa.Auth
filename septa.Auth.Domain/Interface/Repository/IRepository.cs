using JetBrains.Annotations;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IBasicRepository<TEntity> where TEntity : class
    {

        Task<TEntity> FindAsync(
            [NotNull] Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<TEntity> GetAsync(
            [NotNull] Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );


        Task DeleteAsync(
            [NotNull] Expression<Func<TEntity, bool>> predicate,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );
    }



}
