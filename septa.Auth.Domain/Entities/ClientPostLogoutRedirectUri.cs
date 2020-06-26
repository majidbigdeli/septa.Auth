using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientPostLogoutRedirectUri  : Entitiy<int>, IAuditableEntity
    {
        public string PostLogoutRedirectUri { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }








}
