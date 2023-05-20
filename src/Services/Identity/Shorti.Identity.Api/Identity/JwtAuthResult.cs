namespace Shorti.Identity.Api.Identity
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; } = null!;
        public RefreshToken RefreshToken { get; set; } = null!;
    }
}
