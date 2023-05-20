using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shorti.Identity.Api.Identity.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetId(this IEnumerable<Claim> claims) =>
            Guid.Parse(claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public static string GetUsername(this IEnumerable<Claim> claims) =>
            claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
    }
}
