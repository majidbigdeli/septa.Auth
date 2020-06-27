using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{
    public class ApiScope : Entitiy<int>, IAuditableEntity
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiScopeClaim> UserClaims { get; set; } = new List<ApiScopeClaim>();
        public List<ApiScopeProperty> Properties { get; set; } = new List<ApiScopeProperty>();

        public virtual ApiScopeClaim FindClaim(string type)
        {
            return UserClaims?.FirstOrDefault(c => c.Type == type);
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new ApiScopeClaim()
            {
                ScopeId = Id,
                Type = type
            });
        }
    }
}
