using Microsoft.EntityFrameworkCore;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Services
{
    public class SiteStatService : ISiteStatService
    {
        private readonly IUnitOfWork _uow;
        private readonly IApplicationUserManager _userManager;
        private readonly DbSet<User> _users;

        public SiteStatService(
            IApplicationUserManager userManager,
            IUnitOfWork uow)
        {
            _userManager = userManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _users = uow.Set<User>();
        }

        public Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake)
        {
            var now = DateTimeOffset.UtcNow;
            var minutes = now.AddMinutes(-minutesToTake);
            return _users.AsNoTracking()
                         .Where(user => user.LastVisitDateTime != null && user.LastVisitDateTime.Value <= now && user.LastVisitDateTime.Value >= minutes)
                         .OrderByDescending(user => user.LastVisitDateTime)
                         .Take(numbersToTake)
                         .ToListAsync();
        }

        public Task<List<User>> GetTodayBirthdayListAsync()
        {
            var now = DateTimeOffset.UtcNow;
            var day = now.Day;
            var month = now.Month;
            return _users.AsNoTracking()
                         .Where(user => user.BirthDate.HasValue && user.IsActive &&
                                user.BirthDate.Value.Day == day && user.BirthDate.Value.Month == month)
                         .ToListAsync();
        }


        public async Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            user.LastVisitDateTime = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(user);
        }
    }

}
