using septa.Auth.Domain.Interface;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace septa.Auth.Domain.Entities
{
    public class UserUsedPassword : Entitiy<long>, IAuditableEntity
    {
        public string HashedPassword { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }
    }



}
