using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace septa.Auth.Domain.Services
{
    public class SeptaClaimsService : DefaultClaimsService
    {
        //public AbpClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger)
        //    : base(profile, logger)
        //{
        //}

        //protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
        //{
        //    var tenantClaim = subject.FindFirst(AbpClaimTypes.TenantId);
        //    if (tenantClaim == null)
        //    {
        //        return base.GetOptionalClaims(subject);
        //    }

        //    return base.GetOptionalClaims(subject).Union(new[] { tenantClaim });
        //}
        public SeptaClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }
    }

}
