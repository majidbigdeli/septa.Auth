using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using septa.Auth.Domain.Hellper;
using System;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class MemoryCacheTicketStore : ITicketStore
    {
        private const string KeyPrefix = "AuthSessionStore-";
        private readonly IMemoryCache _cache;

        public MemoryCacheTicketStore(IMemoryCache cache)
        {
            _cache = cache;
            _cache.CheckArgumentIsNull(nameof(_cache));
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = $"{KeyPrefix}{Guid.NewGuid().ToString("N")}";
            await RenewAsync(key, ticket);
            return key;
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new MemoryCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;

            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }

            if (ticket.Properties.AllowRefresh.GetValueOrDefault(false))
            {
                options.SetSlidingExpiration(TimeSpan.FromMinutes(60));//TODO: configurable.
            }

            _cache.Set(key, ticket, options);

            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            _cache.TryGetValue(key, out ticket);
            return Task.FromResult(ticket);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(0);
        }
    }


}
