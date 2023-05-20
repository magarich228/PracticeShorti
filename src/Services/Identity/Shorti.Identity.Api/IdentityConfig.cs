using IdentityServer4.Models;

namespace Shorti.Identity.Api
{
    public static class IdentityConfig
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ("video-api"),
                new ("users-api")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityServer4.Models.IdentityResources.Profile()
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "react-client",
                    ClientSecrets = { new Secret("jopesecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes =
                    {
                        "video-api",
                        "users-api"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("video-api"),
                new ApiScope("users-api")
            };
    }
}
