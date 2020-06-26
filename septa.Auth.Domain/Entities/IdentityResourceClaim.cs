namespace septa.Auth.Domain.Entities
{
    public class IdentityResourceClaim : IdentityServerUserClaim
    {
        public int IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; }
    }








}
