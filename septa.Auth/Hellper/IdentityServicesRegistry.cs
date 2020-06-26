using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Settings;
using System;
using System.Collections.Generic;

namespace septa.Auth.Hellper
{
    public static class IdentityServicesRegistry
    {
        public static void AddCustomIdentityServices(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddIdentityOptions(siteSettings);
            services.AddCustomServices();
            services.AddCustomTicketStore(siteSettings);
        //    services.AddDynamicPermissions();
        //    services.AddCustomDataProtection(siteSettings);
        }

        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var identityDbInitialize = scope.ServiceProvider.GetService<IIdentityDbInitializer>();
                identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            }
        }


        public static SiteSettings GetSiteSettings(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var siteSettingsOptions = provider.GetService<IOptionsSnapshot<SiteSettings>>();
            siteSettingsOptions.CheckArgumentIsNull(nameof(siteSettingsOptions));
            var siteSettings = siteSettingsOptions.Value;
            siteSettings.CheckArgumentIsNull(nameof(siteSettings));
            return siteSettings;
        }
    }

}
