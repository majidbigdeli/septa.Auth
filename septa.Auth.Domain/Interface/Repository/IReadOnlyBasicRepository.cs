using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IReadOnlyBasicRepository<TEntity> : IRepository where TEntity : class
    {

        Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(CancellationToken cancellationToken = default);
    }
}
