using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ApiResourceScope : Entitiy<int>, IAuditableEntity
    {
        public string Scope { get; set; }

        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }








}
