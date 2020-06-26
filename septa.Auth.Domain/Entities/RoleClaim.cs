using Microsoft.AspNetCore.Identity;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class RoleClaim : IdentityRoleClaim<long>, IAuditableEntity
    {
        public virtual Role Role { get; set; }
    }

}
