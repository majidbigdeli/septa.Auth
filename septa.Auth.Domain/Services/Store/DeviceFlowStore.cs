using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using septa.Auth.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    /// <summary>
    /// Implementation of IDeviceFlowStore thats uses EF.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IDeviceFlowStore" />
    public class DeviceFlowStore : IDeviceFlowStore
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersistentGrantSerializer serializer;


        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFlowStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serializer">The serializer</param>
        /// <param name="logger">The logger.</param>
        public DeviceFlowStore(
            IUnitOfWork unitOfWork,
            IPersistentGrantSerializer Serializer,
            ILogger<DeviceFlowStore> logger)
        {
            _unitOfWork = unitOfWork;
            serializer = Serializer;
            Logger = logger;
        }

        /// <summary>
        /// Stores the device authorization request.
        /// </summary>
        /// <param name="deviceCode">The device code.</param>
        /// <param name="userCode">The user code.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public virtual async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            _unitOfWork.Set<Entities.DeviceFlowCodes>().Add(ToEntity(data, deviceCode, userCode));

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Finds device authorization by user code.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <returns></returns>
        public virtual async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var deviceFlowCodes = await _unitOfWork.Set<Entities.DeviceFlowCodes>().AsNoTracking().FirstOrDefaultAsync(x => x.UserCode == userCode);
            var model = ToModel(deviceFlowCodes?.Data);

            Logger.LogDebug("{userCode} found in database: {userCodeFound}", userCode, model != null);

            return model;
        }

        /// <summary>
        /// Finds device authorization by device code.
        /// </summary>
        /// <param name="deviceCode">The device code.</param>
        /// <returns></returns>
        public virtual async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = await _unitOfWork.Set<Entities.DeviceFlowCodes>().AsNoTracking().FirstOrDefaultAsync(x => x.DeviceCode == deviceCode);
            var model = ToModel(deviceFlowCodes?.Data);

            Logger.LogDebug("{deviceCode} found in database: {deviceCodeFound}", deviceCode, model != null);

            return model;
        }

        /// <summary>
        /// Updates device authorization, searching by user code.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public virtual async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            var existing = await _unitOfWork.Set<Entities.DeviceFlowCodes>().SingleOrDefaultAsync(x => x.UserCode == userCode);
            if (existing == null)
            {
                Logger.LogError("{userCode} not found in database", userCode);
                throw new InvalidOperationException("Could not update device code");
            }

            var entity = ToEntity(data, existing.DeviceCode, userCode);
            Logger.LogDebug("{userCode} found in database", userCode);

            existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;
            existing.Data = entity.Data;

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning("exception updating {userCode} user code in database: {error}", userCode, ex.Message);
            }
        }

        /// <summary>
        /// Removes the device authorization, searching by device code.
        /// </summary>
        /// <param name="deviceCode">The device code.</param>
        /// <returns></returns>
        public virtual async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = await _unitOfWork.Set<Entities.DeviceFlowCodes>().FirstOrDefaultAsync(x => x.DeviceCode == deviceCode);

            if (deviceFlowCodes != null)
            {
                Logger.LogDebug("removing {deviceCode} device code from database", deviceCode);

                _unitOfWork.Set<Entities.DeviceFlowCodes>().Remove(deviceFlowCodes);

                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Logger.LogInformation("exception removing {deviceCode} device code from database: {error}", deviceCode, ex.Message);
                }
            }
            else
            {
                Logger.LogDebug("no {deviceCode} device code found in database", deviceCode);
            }
        }

        /// <summary>
        /// Converts a model to an entity.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="deviceCode"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        protected Entities.DeviceFlowCodes ToEntity(DeviceCode model, string deviceCode, string userCode)
        {
            if (model == null || deviceCode == null || userCode == null) return null;

            return new Entities.DeviceFlowCodes
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = serializer.Serialize(model)
            };
        }

        /// <summary>
        /// Converts a serialized DeviceCode to a model.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected DeviceCode ToModel(string entity)
        {
            if (entity == null) return null;

            return serializer.Deserialize<DeviceCode>(entity);
        }
    }
}

