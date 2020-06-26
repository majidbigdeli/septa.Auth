using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Mapper;
using septa.Auth.Domain.Services;
using septa.Auth.Domain.Settings;
using septa.Auth.Domain.Settings.Enum;
using System.Collections.Generic;

namespace septa.Auth.Hellper
{
    public static class CustomTicketStoreExtensions
    {
        public static IServiceCollection AddCustomTicketStore(
            this IServiceCollection services, SiteSettings siteSettings)
        {
            // To manage large identity cookies
            var cookieOptions = siteSettings.CookieOptions;
            if (cookieOptions.UseDistributedCacheTicketStore && isActiveDatabaseSqlServer(siteSettings))
            {
                services.AddDistributedSqlServerCache(options =>
                {
                    var cacheOptions = cookieOptions.DistributedSqlServerCacheOptions;
                    var connectionString = string.IsNullOrWhiteSpace(cacheOptions.ConnectionString) ?
                            siteSettings.GetDbConnectionString() :
                            cacheOptions.ConnectionString;
                    options.ConnectionString = connectionString;
                    options.SchemaName = cacheOptions.SchemaName;
                    options.TableName = cacheOptions.TableName;
                });
                services.AddScoped<ITicketStore, DistributedCacheTicketStore>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            }

            return services;
        }

        private static bool isActiveDatabaseSqlServer(SiteSettings siteSettings)
        {
            return siteSettings.ActiveDatabase == ActiveDatabase.SqlServer;
        }
    }


    public static class Config
    {
        public static void GetIdentityResources()
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

        }


    }

}
