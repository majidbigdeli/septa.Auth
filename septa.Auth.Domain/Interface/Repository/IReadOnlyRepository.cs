using System;
using System.Linq;
using System.Linq.Expressions;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IReadOnlyRepository<TEntity> : IQueryable<TEntity>, IReadOnlyBasicRepository<TEntity>
    where TEntity : class
    {
        IQueryable<TEntity> WithDetails();
        IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors);
    }
}
