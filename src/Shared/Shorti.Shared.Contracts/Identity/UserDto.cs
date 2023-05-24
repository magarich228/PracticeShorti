namespace Shorti.Shared.Contracts.Identity
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string AvatarPath { get; set; } = null!;
    }
}
