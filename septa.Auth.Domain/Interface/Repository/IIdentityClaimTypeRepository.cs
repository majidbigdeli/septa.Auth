using septa.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IIdentityClaimTypeRepository : IEfCoreRepository<IdentityClaimType, Guid>
    {
        Task<bool> AnyAsync(string name, Guid? ignoredId = null);

        Task<List<IdentityClaimType>> GetListAsync(
          string sorting,
          int maxResultCount,
          int skipCount,
          string filter);
    }



}
