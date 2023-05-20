using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shorti.Identity.Api.Identity.Abstractions
{
    public interface IJwtIdentityManager
    {
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
        JwtAuthResult GenerateTokens(Guid id, string username, Claim[] claims, DateTime now);
        JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
        void RemoveExpiredRefreshTokens(DateTime now);
        void RemoveRefreshTokenByUser(Guid userId, string username);
        (ClaimsPrincipal claims, JwtSecurityToken token) DecodeJwtToken(string token);
    }
}
