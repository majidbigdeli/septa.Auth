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
using System.Linq.Expressions;

namespace septa.Auth.Domain.Repository
{
    public class ApiResourceRepository : EfCoreRepository<ApiResource, int>, IApiResourceRepository
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
                        where api.Scopes.Any(x => scopeNames.Contains(x.Scope))
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

        public async Task<bool> CheckNameExistAsync(string name, int? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(ar => ar.Id != expectedId && ar.Name == name, cancellationToken: cancellationToken);
        }

        public override async Task DeleteAsync(int id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var scopeClaims = _unitOfWork.Set<ApiScopeClaim>().Where(sc => sc.ScopeId == id);

            foreach (var scopeClaim in scopeClaims)
            {
                _unitOfWork.Set<ApiScopeClaim>().Remove(scopeClaim);
            }

            var scopes = _unitOfWork.Set<ApiScope>().Where(s => s.Id == id);

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

    public class ClientRepository : EfCoreRepository<Client, int>, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public virtual async Task<Client> FindByCliendIdAsync(
          string clientId,
          bool includeDetails = true,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DbSet.IncludeDetails(includeDetails)
                .FirstOrDefaultAsync<Client>((Expression<Func<Client, bool>>)(x => x.ClientId == clientId)
                , this.GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public virtual async Task<List<Client>> GetListAsync(
          string sorting,
          int skipCount,
          int maxResultCount,
          bool includeDetails = false,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DbSet.IncludeDetails(includeDetails).OrderBy<Client>(sorting ?? "ClientName desc").PageBy<Client>(skipCount, maxResultCount).ToListAsync<Client>(this.GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public virtual async Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(
          CancellationToken cancellationToken = default(CancellationToken))
        {
            ClientRepository clientRepository = this;
            return await _unitOfWork.Set<ClientCorsOrigin>()
                .Select<ClientCorsOrigin, string>((Expression<Func<ClientCorsOrigin, string>>)(x => x.Origin)).Distinct<string>().ToListAsync<string>(this.GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public virtual async Task<bool> CheckClientIdExistAsync(
          string clientId,
          int? expectedId = null,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DbSet.AnyAsync<Client>((Expression<Func<Client, bool>>)(c => (int?)c.Id != expectedId && c.ClientId == clientId), cancellationToken).ConfigureAwait(false);
        }

        public override IQueryable<Client> WithDetails()
        {
            return this.GetQueryable().IncludeDetails(true);
        }
    }
}
