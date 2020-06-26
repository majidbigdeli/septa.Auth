using Microsoft.AspNetCore.Identity;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class UserLogin : IdentityUserLogin<long>, IAuditableEntity
    {
        public virtual User User { get; set; }
    }

}
