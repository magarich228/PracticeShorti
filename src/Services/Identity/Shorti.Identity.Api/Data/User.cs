namespace Shorti.Identity.Api.Data;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string AvatarPath { get; set; } = null!;
}