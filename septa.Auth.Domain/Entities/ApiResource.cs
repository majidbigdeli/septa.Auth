using System;
using JetBrains.Annotations;
using IdentityServer4;
using System.Collections.Generic;
using System.Linq;
using septa.Auth.Domain.Interface;

namespace septa.Auth.Domain.Entities
{


    public class ApiResource : Entitiy<int>, IAuditableEntity
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiResourceSecret> Secrets { get; set; } = new List<ApiResourceSecret>();
        public List<ApiResourceScope> Scopes { get; set; } = new List<ApiResourceScope>();
        public List<ApiResourceClaim> UserClaims { get; set; } = new List<ApiResourceClaim>();
        public List<ApiResourceProperty> Properties { get; set; } = new List<ApiResourceProperty>();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }

        public virtual ApiResourceClaim FindClaim(string type)
        {
            return UserClaims?.FirstOrDefault(c => c.Type == type);
        }

        public virtual void RemoveAllSecrets()
        {
            Secrets.Clear();
        }

        public virtual void RemoveAllUserClaims()
        {
            UserClaims.Clear();
        }

        public virtual void RemoveClaim(string type)
        {
            UserClaims.RemoveAll(c => c.Type == type);
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new ApiResourceClaim()
            {
                ApiResourceId = Id,
                Type = type
            });
        }

        public virtual void RemoveScope(string name)
        {
            Scopes.RemoveAll(r => r.Scope == name);
        }

        public virtual ApiResourceScope FindScope(string name)
        {
            return Scopes.FirstOrDefault(r => r.Scope == name);
        }

    }
}
