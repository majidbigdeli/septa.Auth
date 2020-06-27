using Microsoft.EntityFrameworkCore;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Linq;
using septa.Auth.Domain.Hellper;
using System;

namespace septa.Auth.Domain.Repository
{
    public class IdentityClaimTypeRepository : EfCoreRepository<IdentityClaimType, Guid>, IIdentityClaimTypeRepository
    {
        public IdentityClaimTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public virtual async Task<bool> AnyAsync(string name, Guid? ignoredId = null)
        {
            return await DbSet
                       .WhereIf(ignoredId != null, ct => ct.Id != ignoredId)
                       .CountAsync(ct => ct.Name == name) > 0;
        }

        public virtual async Task<List<IdentityClaimType>> GetListAsync(string sorting, int maxResultCount, int skipCount, string filter)
        {
            var identityClaimTypes = await DbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                )
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();

            return identityClaimTypes;
        }
    }


}
