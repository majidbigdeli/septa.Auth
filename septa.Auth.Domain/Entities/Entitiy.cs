using septa.Auth.Domain.Interface;
using septa.Auth.Domain.Interface.Entity;

namespace septa.Auth.Domain.Entities
{
    public class Entitiy<Tkey> : IEntity<Tkey>
    {
        public Tkey Id { get; set; }
    }


}
