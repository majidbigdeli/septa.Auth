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

namespace septa.Auth.Domain.Repository
{
    public class ApiResourceRepository : EfCoreRepository<ApiResource,Guid>, IApiResourceRepository
    {
        public ApiResourceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public virtual async Task<ApiResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = from apiResource in DbSet.IncludeDetails(includeDetails)
                        where apiResource.Name == name
                        select apiResource;

            return await query
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListByScopesAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from api in DbSet.IncludeDetails(includeDetails)
                        where api.Scopes.Any(x => scopeNames.Contains(x.Name))
                        select api;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<ApiResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails).OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<List<ApiResource>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(ar => ar.Id != expectedId && ar.Name == name, cancellationToken: cancellationToken);
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var scopeClaims = _unitOfWork.Set<ApiScopeClaim>().Where(sc => sc.ApiResourceId == id);

            foreach (var scopeClaim in scopeClaims)
            {
                _unitOfWork.Set<ApiScopeClaim>().Remove(scopeClaim);
            }

            var scopes = _unitOfWork.Set<ApiScope>().Where(s => s.ApiResourceId == id);

            foreach (var scope in scopes)
            {
                _unitOfWork.Set<ApiScope>().Remove(scope);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        public override IQueryable<ApiResource> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }

}
