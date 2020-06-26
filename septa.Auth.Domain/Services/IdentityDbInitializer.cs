﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using septa.Auth.Domain.Constant;
using septa.Auth.Domain.Contexts;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Settings;
using septa.Auth.Domain.Settings.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class IdentityDbInitializer : IIdentityDbInitializer
    {
        private readonly IOptionsSnapshot<SiteSettings> _adminUserSeedOptions;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly ILogger<IdentityDbInitializer> _logger;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IServiceScopeFactory _scopeFactory;

        public IdentityDbInitializer(
            IApplicationUserManager applicationUserManager,
            IServiceScopeFactory scopeFactory,
            IApplicationRoleManager roleManager,
            IOptionsSnapshot<SiteSettings> adminUserSeedOptions,
            ILogger<IdentityDbInitializer> logger
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
    }




}
