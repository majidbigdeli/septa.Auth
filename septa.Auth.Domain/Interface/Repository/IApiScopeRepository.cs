using septa.Auth.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IApiScopeRepository : IEfCoreRepository<ApiScope>
    {

        Task<List<ApiScope>> GetListAsync(
            string sorting,
            int skipCount,
            int maxResultCount,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );


    }



}
