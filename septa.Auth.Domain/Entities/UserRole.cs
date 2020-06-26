using Microsoft.AspNetCore.Identity;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class UserRole : IdentityUserRole<long>, IAuditableEntity
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }

}
