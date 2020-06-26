using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientRedirectUri : Entitiy<int>, IAuditableEntity
    {
        public string RedirectUri { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }








}
