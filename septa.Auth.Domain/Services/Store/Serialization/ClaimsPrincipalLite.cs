namespace septa.Auth.Domain.Services.Store.Serialization
{
    public class ClaimsPrincipalLite
    {
        public string AuthenticationType { get; set; }
        public ClaimLite[] Claims { get; set; }
    }
}
