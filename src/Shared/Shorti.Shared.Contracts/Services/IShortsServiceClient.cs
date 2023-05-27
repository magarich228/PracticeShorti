using Shorti.Shared.Contracts.Shorts;

namespace Shorti.Shared.Contracts.Services
{
    public interface IShortsServiceClient
    {
        Task<ShortVideoDto?> GetShortByIdAsync(Guid shortId);
        Task<IEnumerable<ShortVideoDto>> GetUserShorts(Guid userId);
    }
}
