namespace septa.Auth.Domain.Entities
{
    public class ApiResourceClaim : IdentityServerUserClaim
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }








}
