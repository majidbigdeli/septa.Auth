using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Interface.Repository;
using septa.Auth.Domain.Interface.seed;
using septa.Auth.Domain.Interface.utilities;
using septa.Auth.Domain.Mapper;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services.seed
{
    public class IdentityResourceDataSeeder : IIdentityResourceDataSeeder
    {
        public IIdentityClaimTypeRepository ClaimTypeRepository { get; }
        public IGuidGenerator GuidGenerator { get; }
        public IIdentityResourceRepository IdentityResourceRepository { get; }

        public IdentityResourceDataSeeder(
            IIdentityClaimTypeRepository claimTypeRepository,
            IIdentityResourceRepository identityResourceRepository,
            IGuidGenerator guidGenerator)
        {
            IdentityResourceRepository = identityResourceRepository;
            GuidGenerator = guidGenerator;
            ClaimTypeRepository = claimTypeRepository;
        }

        public virtual async Task CreateStandardResourcesAsync()
        {
            var resources = new[]
            {
                new IdentityServer4.Models.IdentityResources.OpenId(),
                new IdentityServer4.Models.IdentityResources.Profile(),
                new IdentityServer4.Models.IdentityResources.Email(),
                new IdentityServer4.Models.IdentityResources.Address(),
                new IdentityServer4.Models.IdentityResources.Phone(),
                new IdentityServer4.Models.IdentityResource("role", "Roles of the user", new[] {"role"})
            };

            foreach (var resource in resources)
            {
                foreach (var claimType in resource.UserClaims)
                {
                    await AddClaimTypeIfNotExistsAsync(claimType);
                }

                await AddIdentityResourceIfNotExistsAsync(resource);
            }
        }


        protected virtual async Task AddIdentityResourceIfNotExistsAsync(IdentityServer4.Models.IdentityResource resource)
        {
            if (await IdentityResourceRepository.CheckNameExistAsync(resource.Name))
            {
                return;
            }

            var entity = resource.ToEntity();
            await IdentityResourceRepository.InsertAsync(entity,true);

        }


        protected virtual async Task AddClaimTypeIfNotExistsAsync(string claimType)
        {
            if (await ClaimTypeRepository.AnyAsync(claimType))
            {
                return;
            }

            await ClaimTypeRepository.InsertAsync(
                new IdentityClaimType(
                    GuidGenerator.Create(),
                    claimType,
                    isStatic: true
                ), true
            );
        }


    }
}
