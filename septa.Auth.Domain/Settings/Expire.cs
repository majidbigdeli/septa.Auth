namespace septa.Auth.Domain.Settings
{
    public class Expire
    {
        public int AccessTokenLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
    }
}
