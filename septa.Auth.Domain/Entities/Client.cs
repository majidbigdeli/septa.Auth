using System;
using System.Collections.Generic;
using septa.Auth.Domain.Interface;
using IdentityServer4.Models;
using System.Linq;
using JetBrains.Annotations;
using IdentityServer4;

namespace septa.Auth.Domain.Entities
{
    public class Client : Entitiy<int>, IAuditableEntity
    {
        public bool Enabled { get; set; } = true;
        public string ClientId { get; set; }
        public string ProtocolType { get; set; } = "oidc";
        public List<ClientSecret> ClientSecrets { get; set; } = new List<ClientSecret>();
        public bool RequireClientSecret { get; set; } = true;
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; } = false;
        public bool AllowRememberConsent { get; set; } = true;
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public List<ClientGrantType> AllowedGrantTypes { get; set; } = new List<ClientGrantType>();
        public bool RequirePkce { get; set; } = true;
        public bool AllowPlainTextPkce { get; set; }
        public bool RequireRequestObject { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<ClientRedirectUri> RedirectUris { get; set; } = new List<ClientRedirectUri>();
        public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; } = new List<ClientPostLogoutRedirectUri>();
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        public bool AllowOfflineAccess { get; set; }
        public List<ClientScope> AllowedScopes { get; set; } = new List<ClientScope>();
        public int IdentityTokenLifetime { get; set; } = 300;
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int AccessTokenLifetime { get; set; } = 3600;
        public int AuthorizationCodeLifetime { get; set; } = 300;
        public int? ConsentLifetime { get; set; } = null;
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        public int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;
        public int AccessTokenType { get; set; } = (int)0; // AccessTokenType.Jwt;
        public bool EnableLocalLogin { get; set; } = true;
        public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; } = new List<ClientIdPRestriction>();
        public bool IncludeJwtId { get; set; }
        public List<ClientClaim> Claims { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; } = "client_";
        public string PairWiseSubjectSalt { get; set; }
        public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; } = new List<ClientCorsOrigin>();
        public List<ClientProperty> Properties { get; set; } = new List<ClientProperty>();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public int DeviceCodeLifetime { get; set; } = 300;
        public bool NonEditable { get; set; }


        public virtual ClientScope FindScope(string scope)
        {
            return AllowedScopes.FirstOrDefault(r => r.Scope == scope);
        }

        public virtual void RemoveScope(string scope)
        {
            AllowedScopes.RemoveAll(r => r.Scope == scope);
        }

        public virtual void RemoveAllScopes()
        {
            AllowedScopes.Clear();
        }

        public virtual void AddScope([NotNull] string scope)
        {
            AllowedScopes.Add(new ClientScope() {
                ClientId = Id,
                Scope = scope
            });
        }

        public virtual void AddPostLogoutRedirectUri([NotNull] string postLogoutRedirectUri)
        {
            PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri() {
                ClientId = Id,
                PostLogoutRedirectUri = postLogoutRedirectUri
            });
        }

        public virtual void RemoveAllCorsOrigins()
        {
            AllowedCorsOrigins.Clear();
        }


        public virtual void RemoveCorsOrigin(string uri)
        {
            AllowedCorsOrigins.RemoveAll(c => c.Origin == uri);
        }

        public virtual void RemoveAllRedirectUris()
        {
            RedirectUris.Clear();
        }

        public virtual void RemoveRedirectUri(string uri)
        {
            RedirectUris.RemoveAll(r => r.RedirectUri == uri);
        }

        public virtual void RemoveAllPostLogoutRedirectUris()
        {
            PostLogoutRedirectUris.Clear();
        }

        public virtual void RemovePostLogoutRedirectUri(string uri)
        {
            PostLogoutRedirectUris.RemoveAll(p => p.PostLogoutRedirectUri == uri);
        }

        public virtual ClientGrantType FindGrantType(string grantType)
        {
            return AllowedGrantTypes.FirstOrDefault(r => r.GrantType == grantType);
        }

        public virtual void AddSecret([NotNull] string value, DateTime? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string description = null)
        {
            ClientSecrets.Add(new ClientSecret() {
                ClientId = Id,
                Value = value,
                Expiration = expiration,
                Type = type,
                Description = description

            });
        }

        public virtual void RemoveSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            ClientSecrets.RemoveAll(s => s.Value == value && s.Type == type);
        }

        public virtual ClientSecret FindSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            return ClientSecrets.FirstOrDefault(s => s.Type == type && s.Value == value);
        }

        public virtual void AddGrantType([NotNull] string grantType)
        {
            AllowedGrantTypes.Add(new ClientGrantType() {
                 ClientId = Id,
                 GrantType = grantType
            });
        }

        public virtual void RemoveAllAllowedGrantTypes()
        {
            AllowedGrantTypes.Clear();
        }

        public virtual void RemoveGrantType(string grantType)
        {
            AllowedGrantTypes.RemoveAll(r => r.GrantType == grantType);
        }


        public virtual ClientCorsOrigin FindCorsOrigin(string uri)
        {
            return AllowedCorsOrigins.FirstOrDefault(c => c.Origin == uri);
        }

        public virtual ClientRedirectUri FindRedirectUri(string uri)
        {
            return RedirectUris.FirstOrDefault(r => r.RedirectUri == uri);
        }

        public virtual ClientPostLogoutRedirectUri FindPostLogoutRedirectUri(string uri)
        {
            return PostLogoutRedirectUris.FirstOrDefault(p => p.PostLogoutRedirectUri == uri);
        }

        public virtual void AddProperty([NotNull] string key, [NotNull] string value)
        {
            Properties.Add(new ClientProperty() {
                ClientId = Id,
                Key = key,
                Value = value
            });
        }

        public virtual void RemoveAllProperties()
        {
            Properties.Clear();
        }


        public virtual void AddRedirectUri([NotNull] string redirectUri)
        {
            RedirectUris.Add(new ClientRedirectUri() {
                ClientId = Id,
                RedirectUri = redirectUri
            });
        }

    }

}
