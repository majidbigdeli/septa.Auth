using IdentityServer4.Services;
using System;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class AbpCorsPolicyService : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            throw new NotImplementedException();
        }
    }

}
