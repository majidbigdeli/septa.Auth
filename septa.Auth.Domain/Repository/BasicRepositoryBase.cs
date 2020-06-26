using septa.Auth.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using septa.Auth.Domain.Threading;
using septa.Auth.Domain.Hellper;

namespace septa.Auth.Domain.Repository
{
    public abstract class BasicRepositoryBase<TEntity> : IBasicRepository<TEntity> where TEntity : class
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        protected BasicRepositoryBase()
        {
            CancellationTokenProvider = NullCancellationTokenProvider.Instance;
        }

        public abstract Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public abstract Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

        public abstract Task<long> GetCountAsync(CancellationToken cancellationToken = default);

        protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return CancellationTokenProvider.FallbackToProvider(preferredValue);
        }
    }
}
