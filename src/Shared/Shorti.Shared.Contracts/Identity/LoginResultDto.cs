namespace Shorti.Shared.Contracts.Identity
{
    public class LoginResultDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
