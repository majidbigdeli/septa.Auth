using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public abstract class Property : Entitiy<int>, IAuditableEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }








}
