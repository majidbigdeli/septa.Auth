using IdentityServer4.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using septa.Auth.Domain.Constant;
using septa.Auth.Domain.Interface;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

namespace septa.Auth.Domain.Services
{
    //public class CorsPolicyService : ICorsPolicyService
    //{
    //    public ILogger<CorsPolicyService> Logger { get; set; }
    //    protected IHybridServiceScopeFactory HybridServiceScopeFactory { get; }
    //    protected IDistributedCache<AllowedCorsOriginsCacheItem> Cache { get; }

    //    public AbpCorsPolicyService(
    //        IDistributedCache<AllowedCorsOriginsCacheItem> cache,
    //        IHybridServiceScopeFactory hybridServiceScopeFactory)
    //    {
    //        Cache = cache;
    //        HybridServiceScopeFactory = hybridServiceScopeFactory;
    //        Logger = NullLogger<CorsPolicyService>.Instance;
    //    }

    //    public virtual async Task<bool> IsOriginAllowedAsync(string origin)
    //    {
    //        var cacheItem = await Cache.GetOrAddAsync(AllowedCorsOriginsCacheItem.AllOrigins, CreateCacheItemAsync);

    //        var isAllowed = cacheItem.AllowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

    //        if (!isAllowed)
    //        {
    //            Logger.LogWarning($"Origin is not allowed: {origin}");
    //        }

    //        return isAllowed;
    //    }

    //    protected virtual async Task<AllowedCorsOriginsCacheItem> CreateCacheItemAsync()
    //    {
    //        // doing this here and not in the ctor because: https://github.com/aspnet/AspNetCore/issues/2377
    //        using (var scope = HybridServiceScopeFactory.CreateScope())
    //        {
    //            var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

    //            return new AllowedCorsOriginsCacheItem
    //            {
    //                AllowedOrigins = (await clientRepository.GetAllDistinctAllowedCorsOriginsAsync()).ToArray()
    //            };
    //        }
    //    }
    //}

}
