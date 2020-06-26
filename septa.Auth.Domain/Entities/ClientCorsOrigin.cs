using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ClientCorsOrigin : Entitiy<int>, IAuditableEntity
    {
        public string Origin { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }








}
