using Shorti.Shared.Contracts.Identity;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shorti.Shared.Contracts.Services
{
    public class IdentityServiceClient : IIdentityServiceClient
    {
        private readonly HttpClient _identityServiceClient;

        public IdentityServiceClient(IHttpClientFactory clientFactory)
        {
            _identityServiceClient = clientFactory.CreateClient("ApiGwHost");
        }

        public async Task<UserDto?> GetCurrentUserAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _identityServiceClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await _identityServiceClient.GetFromJsonAsync<UserDto>("api/users/current");
        }
    }
}
