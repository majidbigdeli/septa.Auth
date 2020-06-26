using septa.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IApiResourceRepository : IEfCoreRepository<ApiResource, Guid>
    {
        Task<ApiResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<List<ApiResource>> GetListByScopesAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<ApiResource>> GetListAsync(
            string sorting,
            int skipCount,
            int maxResultCount,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<ApiResource>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<bool> CheckNameExistAsync(
            string name,
            Guid? expectedId = null,
            CancellationToken cancellationToken = default
        );
    }


}
