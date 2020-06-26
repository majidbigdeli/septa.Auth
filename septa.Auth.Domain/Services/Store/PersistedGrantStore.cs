using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    /// <summary>
    /// Implementation of IPersistedGrantStore thats uses EF.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IPersistedGrantStore" />
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The DbContext.
        /// </summary>

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedGrantStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        public PersistedGrantStore(IUnitOfWork unitOfWork, ILogger<PersistedGrantStore> logger)
        {
            _unitOfWork = unitOfWork;
            Logger = logger;
        }

        /// <inheritdoc/>
        public virtual async Task StoreAsync(PersistedGrant token)
        {
            var existing = await _unitOfWork.Set<Entities.PersistedGrant>().SingleOrDefaultAsync(x => x.Key == token.Key);
            if (existing == null)
            {
                Logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

                var persistedGrant = token.ToEntity();
                _unitOfWork.Set<Entities.PersistedGrant>().Add(persistedGrant);
            }
            else
            {
                Logger.LogDebug("{persistedGrantKey} found in database", token.Key);

                token.UpdateEntity(existing);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning("exception updating {persistedGrantKey} persisted grant in database: {error}", token.Key, ex.Message);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await _unitOfWork.Set<Entities.PersistedGrant>().AsNoTracking().FirstOrDefaultAsync(x => x.Key == key);
            var model = persistedGrant?.ToModel();

            Logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);

            return model;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            filter.Validate();

            var persistedGrants = await Filter(filter).ToArrayAsync();
            var model = persistedGrants.Select(x => x.ToModel());

            Logger.LogDebug("{persistedGrantCount} persisted grants found for {@filter}", persistedGrants.Length, filter);

            return model;
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAsync(string key)
        {
            var persistedGrant = await _unitOfWork.Set<Entities.PersistedGrant>().FirstOrDefaultAsync(x => x.Key == key);
            if (persistedGrant != null)
            {
                Logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

                _unitOfWork.Set<Entities.PersistedGrant>().Remove(persistedGrant);

                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Logger.LogInformation("exception removing {persistedGrantKey} persisted grant from database: {error}", key, ex.Message);
                }
            }
            else
            {
                Logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            filter.Validate();

            var persistedGrants = await Filter(filter).ToArrayAsync();

            Logger.LogDebug("removing {persistedGrantCount} persisted grants from database for {@filter}", persistedGrants.Length, filter);

            _unitOfWork.Set<Entities.PersistedGrant>().RemoveRange(persistedGrants);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogInformation("removing {persistedGrantCount} persisted grants from database for subject {@filter}: {error}", persistedGrants.Length, filter, ex.Message);
            }
        }


        private IQueryable<Entities.PersistedGrant> Filter(PersistedGrantFilter filter)
        {
            var query = _unitOfWork.Set<Entities.PersistedGrant>().AsQueryable();

            if (!String.IsNullOrWhiteSpace(filter.ClientId))
            {
                query = query.Where(x => x.ClientId == filter.ClientId);
            }
            if (!String.IsNullOrWhiteSpace(filter.SessionId))
            {
                query = query.Where(x => x.SessionId == filter.SessionId);
            }
            if (!String.IsNullOrWhiteSpace(filter.SubjectId))
            {
                query = query.Where(x => x.SubjectId == filter.SubjectId);
            }
            if (!String.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(x => x.Type == filter.Type);
            }

            return query;
        }
    }

}
