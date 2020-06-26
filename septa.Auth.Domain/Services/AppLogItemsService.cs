using Microsoft.EntityFrameworkCore;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class AppLogItemsService : IAppLogItemsService
    {
        private readonly DbSet<AppLogItem> _appLogItems;
        private readonly IUnitOfWork _uow;
        public AppLogItemsService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _appLogItems = _uow.Set<AppLogItem>();
        }

        public Task DeleteAllAsync(string logLevel = "")
        {
            if (string.IsNullOrWhiteSpace(logLevel))
            {
                _appLogItems.RemoveRange(_appLogItems);
            }
            else
            {
                var query = _appLogItems.Where(l => l.LogLevel == logLevel);
                _appLogItems.RemoveRange(query);
            }

            return _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int logItemId)
        {
            var itemToRemove = await _appLogItems.FirstOrDefaultAsync(x => x.Id.Equals(logItemId));
            if (itemToRemove != null)
            {
                _appLogItems.Remove(itemToRemove);
                await _uow.SaveChangesAsync();
            }
        }

        public Task DeleteOlderThanAsync(DateTimeOffset cutoffDateUtc, string logLevel = "")
        {
            if (string.IsNullOrWhiteSpace(logLevel))
            {
                var query = _appLogItems.Where(l => l.CreatedDateTime < cutoffDateUtc);
                _appLogItems.RemoveRange(query);
            }
            else
            {
                var query = _appLogItems.Where(l => l.CreatedDateTime < cutoffDateUtc && l.LogLevel == logLevel);
                _appLogItems.RemoveRange(query);
            }

            return _uow.SaveChangesAsync();
        }


        public Task<int> GetCountAsync(string logLevel = "")
        {
            return string.IsNullOrWhiteSpace(logLevel) ?
                            _appLogItems.CountAsync() :
                            _appLogItems.Where(l => l.LogLevel == logLevel).CountAsync();
        }
    }


}
