using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class IdentityServerUserClaim : Entitiy<int>, IAuditableEntity
    {
        public string Type { get; set; }

    }








}
