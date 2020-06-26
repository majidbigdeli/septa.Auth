using System;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public abstract class Secret : Entitiy<int> , IAuditableEntity
    {
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; } = "SharedSecret";
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }








}
