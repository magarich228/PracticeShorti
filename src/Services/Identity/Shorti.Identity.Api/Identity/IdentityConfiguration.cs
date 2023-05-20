namespace Shorti.Identity.Api.Identity
{
    public class IdentityConfiguration
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string ServiceKey { get; set; } = null!;
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
