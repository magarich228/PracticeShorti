using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shorti.Identity.Api.Data;
using Shorti.Shared.Contracts.Identity;
using IdentityModel.Client;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityController(
            IIdentityServerInteractionService identityServerInteractionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IHttpClientFactory httpClientFactory)
        {
            _identityServerInteractionService = identityServerInteractionService;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Host()
        {
            var identityClient = _httpClientFactory.CreateClient("IdentityClient");
            identityClient.BaseAddress = new Uri("http://localhost:5064/");

            var discoveryDocument = await identityClient.GetDiscoveryDocumentAsync();

            identityClient.SetBasicAuthenticationOAuth("magarich228", "@Jope123");

            var client = IdentityConfig.Clients.First();
            var tokenResponse = await identityClient.RequestTokenAsync(new TokenRequest
            {
                ClientId = client.ClientId,
                ClientSecret = client.ClientSecrets.First().Value,
                GrantType = client.AllowedGrantTypes.First()
            });

            return Ok(tokenResponse);
        }
    }
}
