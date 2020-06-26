using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientClaim : Entitiy<int>, IAuditableEntity
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }








}
