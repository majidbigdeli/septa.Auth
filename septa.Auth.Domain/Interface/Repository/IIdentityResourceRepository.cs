using septa.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IIdentityResourceRepository : IEfCoreRepository<IdentityResource, Guid>
    {
        Task<List<IdentityResource>> GetListByScopesAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityResource>> GetListAsync(
            string sorting,
            int skipCount,
            int maxResultCount,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<IdentityResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<bool> CheckNameExistAsync(
            string name,
            Guid? expectedId = null,
            CancellationToken cancellationToken = default
         );
    }



}
