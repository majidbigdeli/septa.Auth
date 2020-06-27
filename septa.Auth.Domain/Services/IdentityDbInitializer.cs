using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using septa.Auth.Domain.Constant;
using septa.Auth.Domain.Contexts;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Interface.Repository;
using septa.Auth.Domain.Interface.seed;
using septa.Auth.Domain.Settings;
using septa.Auth.Domain.Settings.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApiResource = septa.Auth.Domain.Entities.ApiResource;
using Client = septa.Auth.Domain.Entities.Client;

namespace septa.Auth.Domain.Services
{
    public class IdentityDbInitializer : IIdentityDbInitializer
    {
        private readonly IOptionsSnapshot<SiteSettings> _adminUserSeedOptions;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly ILogger<IdentityDbInitializer> _logger;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IServiceScopeFactory _scopeFactory;

        public IdentityDbInitializer(
            IApplicationUserManager applicationUserManager,
            IServiceScopeFactory scopeFactory,
            IApplicationRoleManager roleManager,
            IOptionsSnapshot<SiteSettings> adminUserSeedOptions,
            ILogger<IdentityDbInitializer> logger,
            IApiResourceRepository apiResourceRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IClientRepository clientRepository,
            IConfiguration configuration
            )
        {
            _applicationUserManager = applicationUserManager;
            _applicationUserManager.CheckArgumentIsNull(nameof(_applicationUserManager));

            _scopeFactory = scopeFactory;
            _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            _roleManager = roleManager;
            _roleManager.CheckArgumentIsNull(nameof(_roleManager));

            _adminUserSeedOptions = adminUserSeedOptions;
            _adminUserSeedOptions.CheckArgumentIsNull(nameof(_adminUserSeedOptions));

            _logger = logger;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _clientRepository = clientRepository;
            _configuration = configuration;
            _logger.CheckArgumentIsNull(nameof(_logger));
        }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    if (_adminUserSeedOptions.Value.ActiveDatabase == ActiveDatabase.InMemoryDatabase)
                    {
                        context.Database.EnsureCreated();
                    }
                    else
                    {
                        context.Database.Migrate();
                    }
                }
            }
        }

        /// <summary>
        /// Adds some default values to the IdentityDb
        /// </summary>
        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                var identityDbSeedData = serviceScope.ServiceProvider.GetService<IIdentityDbInitializer>();

                // How to add initial data to the DB directly
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    //SaveStandardIdentityResources(context).Wait();

                    if (!context.Roles.Any())
                    {
                        var adminRoleResult = _roleManager.CreateAsync(new Role(ConstantRoles.Admin)).Result;
                        if (adminRoleResult == IdentityResult.Failed())
                        {
                            _logger.LogError($"adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                        }
                    }
                    if (!context.Roles.Any(x => x.Name == ConstantRoles.Admin))
                    {
                        var adminRoleResult = _roleManager.CreateAsync(new Role(ConstantRoles.Admin)).Result;
                        if (adminRoleResult == IdentityResult.Failed())
                        {
                            _logger.LogError($"adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                        }
                    }
                    if (!context.Roles.Any(x => x.Name == ConstantRoles.User))
                    {
                        var adminRoleResult = _roleManager.CreateAsync(new Role(ConstantRoles.User)).Result;
                        if (adminRoleResult == IdentityResult.Failed())
                        {
                            _logger.LogError($"userRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                        }
                    }
                    if (!context.Roles.Any(x => x.Name == ConstantRoles.Partner))
                    {
                        var adminRoleResult = _roleManager.CreateAsync(new Role(ConstantRoles.Partner)).Result;
                        if (adminRoleResult == IdentityResult.Failed())
                        {
                            _logger.LogError($"partnerRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                        }
                    }
                }

                var result = identityDbSeedData.SeedDatabaseWithAdminUserAsync().Result;
                if (result == IdentityResult.Failed())
                {
                    throw new InvalidOperationException(result.DumpErrors());
                }

                SeedIdentityServer().Wait();

            }
        }

        public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
        {
            var adminUserSeed = _adminUserSeedOptions.Value.AdminUserSeed;
            adminUserSeed.CheckArgumentIsNull(nameof(adminUserSeed));

            var name = adminUserSeed.Username;
            var password = adminUserSeed.Password;
            var email = adminUserSeed.Email;
            var roleName = adminUserSeed.RoleName;

            var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);

            var adminUser = await _applicationUserManager.FindByNameAsync(name);
            if (adminUser != null)
            {
                _logger.LogInformation($"{thisMethodName}: adminUser already exists.");
                return IdentityResult.Success;
            }

            //Create the `Admin` Role if it does not exist
            var adminRole = await _roleManager.FindByNameAsync(roleName);
            if (adminRole == null)
            {
                adminRole = new Role(roleName);
                var adminRoleResult = await _roleManager.CreateAsync(adminRole);
                if (adminRoleResult == IdentityResult.Failed())
                {
                    _logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                    return IdentityResult.Failed();
                }
            }
            else
            {
                _logger.LogInformation($"{thisMethodName}: adminRole already exists.");
            }

            adminUser = new User
            {
                UserName = name,
                Email = email,
                EmailConfirmed = true,
                IsEmailPublic = true,
                LockoutEnabled = true,


            };
            var adminUserResult = await _applicationUserManager.CreateAsync(adminUser, password);
            if (adminUserResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            var setLockoutResult = await _applicationUserManager.SetLockoutEnabledAsync(adminUser, enabled: false);
            if (setLockoutResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed. {setLockoutResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            var addToRoleResult = await _applicationUserManager.AddToRoleAsync(adminUser, adminRole.Name);
            if (addToRoleResult == IdentityResult.Failed())
            {
                _logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public async Task SeedIdentityServer()
        {
            await _identityResourceDataSeeder.CreateStandardResourcesAsync();
            await CreateApiResourcesAsync();
            await CreateClientsAsync();

        }

        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[]
            {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };

            await CreateApiResourceAsync("PerformanceEvaluation", commonApiUserClaims);
        }
        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource()
                    {
                        Name = name,
                        DisplayName = name + "API"
                    }, autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task CreateClientsAsync()
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address",
                "PerformanceEvaluation"
            };

            var configurationSection = _configuration.GetSection("IdentityServer:Clients");

            //Web Client
            var webClientId = configurationSection["PerformanceEvaluation_Web:ClientId"];
            if (!webClientId.IsNullOrWhiteSpace())
            {
                var webClientRootUrl = configurationSection["PerformanceEvaluation_Web:RootUrl"].EnsureEndsWith('/');

                /* PerformanceEvaluation_Web client is only needed if you created a tiered
                 * solution. Otherwise, you can delete this client. */

                await CreateClientAsync(
                    webClientId,
                    commonScopes,
                    new[] { "hybrid" },
                    (configurationSection["PerformanceEvaluation_Web:ClientSecret"] ?? "1q2w3e*").Sha256(),
                    redirectUri: $"{webClientRootUrl}signin-oidc",
                    postLogoutRedirectUri: $"{webClientRootUrl}signout-callback-oidc"
                );
            }

            //Console Test Client
            var consoleClientId = configurationSection["PerformanceEvaluation_App:ClientId"];
            if (!consoleClientId.IsNullOrWhiteSpace())
            {
                await CreateClientAsync(
                    consoleClientId,
                    commonScopes,
                    new[] { "password", "client_credentials" },
                    (configurationSection["PerformanceEvaluation_App:ClientSecret"] ?? "1q2w3e*").Sha256()
                );
            }
        }

        private async Task<Client> CreateClientAsync(
    string name,
    IEnumerable<string> scopes,
    IEnumerable<string> grantTypes,
    string secret,
    string redirectUri = null,
    string postLogoutRedirectUri = null,
    IEnumerable<string> permissions = null)
        {
            var client = await _clientRepository.FindByCliendIdAsync(name);
            if (client == null)
            {
                client = await _clientRepository.InsertAsync(
                    new Client()
                    {
                        ClientId = name,
                        ClientName = name,
                        ProtocolType = "oidc",
                        Description = name,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowOfflineAccess = true,
                        AbsoluteRefreshTokenLifetime = 86400, //365 days
                        AccessTokenLifetime = 86400, //365 days
                        AuthorizationCodeLifetime = 300,
                        IdentityTokenLifetime = 300,
                        RequireConsent = false
                    },
                    autoSave: true
                );
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (client.FindSecret(secret) == null)
            {
                client.AddSecret(secret);
            }

            if (redirectUri != null)
            {
                if (client.FindRedirectUri(redirectUri) == null)
                {
                    client.AddRedirectUri(redirectUri);
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                }
            }

            //if (permissions != null)
            //{
            //    await _permissionDataSeeder.SeedAsync(
            //        ClientPermissionValueProvider.ProviderName,
            //        name,
            //        permissions
            //    );
            //}

            return await _clientRepository.UpdateAsync(client);
        }
    }
}
