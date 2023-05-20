namespace Shorti.Identity.Api.Identity
{
    public class RefreshToken
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string TokenString { get; set; } = null!;
        public DateTime ExpireAt { get; set; }
    }
}
