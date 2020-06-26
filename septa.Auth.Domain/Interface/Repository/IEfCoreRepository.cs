using Microsoft.EntityFrameworkCore;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        DbSet<TEntity> DbSet { get; }
    }

    public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity>
       where TEntity : class
    {

    }

}
