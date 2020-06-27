using septa.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface.Repository
{
    public interface IClientRepository : IEfCoreRepository<Client, int>
    {
        Task<Client> FindByCliendIdAsync(
            string clientId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Client>> GetListAsync(
          string sorting,
          int skipCount,
          int maxResultCount,
          bool includeDetails = false,
          CancellationToken cancellationToken = default(CancellationToken));

        Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(
          CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> CheckClientIdExistAsync(
          string clientId,
          int? expectedId = null,
          CancellationToken cancellationToken = default(CancellationToken));

    }
}
