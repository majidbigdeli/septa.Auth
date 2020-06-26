using System;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface
{
    public interface IAppLogItemsService
    {
        Task DeleteAllAsync(string logLevel = "");
        Task DeleteAsync(int logItemId);
        Task DeleteOlderThanAsync(DateTimeOffset cutoffDateUtc, string logLevel = "");
        Task<int> GetCountAsync(string logLevel = "");
    }


}
