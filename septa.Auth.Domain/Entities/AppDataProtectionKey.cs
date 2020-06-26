using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class AppDataProtectionKey : Entitiy<int>, IAuditableEntity
    {
        public string FriendlyName { get; set; }
        public string XmlData { get; set; }
    }
}
