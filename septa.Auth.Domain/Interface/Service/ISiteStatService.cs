using septa.Auth.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace septa.Auth.Domain.Interface
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

    }


}
