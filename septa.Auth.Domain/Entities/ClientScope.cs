using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientScope : Entitiy<int>, IAuditableEntity
    {
        public string Scope { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }

}
