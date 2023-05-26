using Shorti.Shared.Contracts.Identity;

namespace Shorti.Shared.Contracts.Services
{
    public interface IIdentityServiceClient
    {
        Task<UserDto?> GetCurrentUserAsync(string accessToken);
    }
}
