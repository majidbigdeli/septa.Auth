using Microsoft.EntityFrameworkCore;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using septa.Auth.Domain.Hellper;
using JetBrains.Annotations;
using System.Linq.Expressions;

namespace septa.Auth.Domain.Repository
{
    public class IdentityResourceRepository : EfCoreRepository<IdentityResource, int>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public virtual async Task<List<IdentityResource>> GetListByScopesAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from identityResource in DbSet.IncludeDetails(includeDetails)
                        where scopeNames.Contains(identityResource.Name)
                        select identityResource;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<IdentityResource> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public virtual async Task<List<IdentityResource>> GetListAsync(string sorting, int skipCount, int maxResultCount,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<IdentityResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> CheckNameExistAsync(string name, int? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(ir => ir.Id != expectedId && ir.Name == name, cancellationToken: cancellationToken);
        }
    }
}
