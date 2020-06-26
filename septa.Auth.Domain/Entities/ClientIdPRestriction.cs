using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientIdPRestriction : Entitiy<int>, IAuditableEntity
    {
        public string Provider { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }








}
