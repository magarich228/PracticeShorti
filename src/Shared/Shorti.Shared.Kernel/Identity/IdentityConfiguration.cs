namespace Shorti.Shared.Kernel.Identity
{
    public class IdentityConfiguration
    {
        public string Issuer { get; set; } = "http://localhost:5064";
        public string Audience { get; set; } = "http://localhost:5283"; //react client
        public string ServiceKey { get; set; } = "i_love_elizabeth_cherepanova";
        public int AccessTokenExpiration { get; set; } = 60;
        public int RefreshTokenExpiration { get; set; } = 60;
    }
}