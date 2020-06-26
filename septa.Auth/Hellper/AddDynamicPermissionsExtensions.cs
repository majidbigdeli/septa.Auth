using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using septa.Auth.Domain.Constant;
using septa.Auth.Domain.Services;

namespace septa.Auth.Hellper
{
    //public static class AddDynamicPermissionsExtensions
    //{
    //    public static IServiceCollection AddDynamicPermissions(this IServiceCollection services)
    //    {
    //        services.AddScoped<IAuthorizationHandler, DynamicPermissionsAuthorizationHandler>();
    //        services.AddAuthorization(opts =>
    //        {
    //            opts.AddPolicy(
    //                name: ConstantPolicies.DynamicPermission,
    //                configurePolicy: policy =>
    //                {
    //                    policy.RequireAuthenticatedUser();
    //                    policy.Requirements.Add(new DynamicPermissionRequirement());
    //                });

    //            opts.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
    //            {
    //                policyAdmin.RequireClaim("role", "dataEventRecords.admin");
    //            });
    //            opts.AddPolicy("admin", policyAdmin =>
    //            {
    //                policyAdmin.RequireClaim("role", "admin");
    //            });
    //            opts.AddPolicy("dataEventRecordsUser", policyUser =>
    //            {
    //                policyUser.RequireClaim("role", "dataEventRecords.user");
    //            });

    //        });

    //        return services;
    //    }
    //}

}
