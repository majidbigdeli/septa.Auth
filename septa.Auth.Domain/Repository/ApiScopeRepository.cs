using Microsoft.EntityFrameworkCore;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using septa.Auth.Domain.Hellper;

namespace septa.Auth.Domain.Repository
{
    public class ApiScopeRepository : EfCoreRepository<ApiScope>, IApiScopeRepository
    {
        public ApiScopeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<ApiScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }




}
