using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientGrantType : Entitiy<int>, IAuditableEntity
    {
        public string GrantType { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }








}
