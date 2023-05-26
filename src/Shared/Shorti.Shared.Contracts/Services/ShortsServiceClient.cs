using Shorti.Shared.Contracts.Shorts;
using System.Net.Http.Json;

namespace Shorti.Shared.Contracts.Services
{
    public class ShortsServiceClient : IShortsServiceClient
    {
        private readonly HttpClient _shortsServiceHost;

        public ShortsServiceClient(IHttpClientFactory httpClientFactory)
        {
            _shortsServiceHost = httpClientFactory.CreateClient("ShortsHost");
        }

        public async Task<ShortVideoDto?> GetShortByIdAsync(Guid shortId)
        {
            var response = await _shortsServiceHost.GetAsync($"api/shorts/{shortId}");

            ShortVideoDto? @short = response.IsSuccessStatusCode ?
                await response.Content.ReadFromJsonAsync<ShortVideoDto>() : 
                default;

            return @short;
        }

        public async Task<IEnumerable<ShortVideoDto>> GetUserShorts(Guid userId)
        {
            var response = await _shortsServiceHost.GetAsync($"api/shorts/user/{userId}");

            IEnumerable<ShortVideoDto> shorts = response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<IEnumerable<ShortVideoDto>>())!:
                new List<ShortVideoDto>();

            return shorts;
        }
    }
}
