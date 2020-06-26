namespace septa.Auth.Domain.Entities
{
    public class ApiScopeClaim : IdentityServerUserClaim
    {
        public int ScopeId { get; set; }
        public ApiScope Scope { get; set; }
    }








}
