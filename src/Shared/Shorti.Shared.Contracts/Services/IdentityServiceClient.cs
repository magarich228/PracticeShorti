using Shorti.Shared.Contracts.Identity;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shorti.Shared.Contracts.Services
{
    public class IdentityServiceClient : IIdentityServiceClient
    {
        private readonly HttpClient _apiGatewayHost;
        private readonly HttpClient _identityHost;

        public IdentityServiceClient(IHttpClientFactory clientFactory)
        {
            _apiGatewayHost = clientFactory.CreateClient("ApiGwHost");
            _identityHost = clientFactory.CreateClient("IdentityHost");
        }

        public async Task<UserDto?> GetCurrentUserAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _apiGatewayHost.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await _apiGatewayHost.GetFromJsonAsync<UserDto>("api/users/current");
        }

        public async Task<UserDto?> GetUserById(Guid userId)
        {
            var response = await _identityHost.GetAsync($"api/users/{userId}");

            UserDto? user = response.IsSuccessStatusCode ?
                await response.Content.ReadFromJsonAsync<UserDto>() :
                default;

            return user;
        }
    }
}
