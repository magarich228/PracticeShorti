using Microsoft.IdentityModel.Tokens;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Identity.Extensions;
using Shorti.Shared.Kernel.Identity;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shorti.Identity.Api.Identity
{
    public class JwtIdentityManager : IJwtIdentityManager
    {
        public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;
        private readonly IdentityConfiguration _identityConfig;
        private readonly byte[] _secret;

        public JwtIdentityManager(IdentityConfiguration identityConfig)
        {
            _identityConfig = identityConfig;
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
            _secret = Encoding.UTF8.GetBytes(identityConfig.ServiceKey);
        }

        public void RemoveExpiredRefreshTokens(DateTime now)
        {
            var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();

            foreach (var expiredToken in expiredTokens)
            {
                _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
            }
        }

        public void RemoveRefreshTokenByUser(Guid userId, string username)
        {
            var refreshTokens = _usersRefreshTokens
                .Where(x => x.Value.UserName == username &&
                    x.Value.UserId == userId)
                .ToList();

            foreach (var refreshToken in refreshTokens)
            {
                _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
            }
        }

        public JwtAuthResult GenerateTokens(Guid id, string username, Claim[] claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims
                ?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)
                ?.Value);

            var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(_secret),
                    SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                _identityConfig.Issuer,
                shouldAddAudienceClaim ? _identityConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_identityConfig.AccessTokenExpiration),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken
            {
                UserId = id,
                UserName = username,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = now.AddMinutes(_identityConfig.RefreshTokenExpiration)
            };

            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            Guid id = principal.Claims.GetId();
            string userName = principal.Claims.GetUsername();

            if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            {
                throw new SecurityTokenException("Invalid token");
            }

            if (existingRefreshToken.UserName != userName || existingRefreshToken.UserId != id || 
                existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return GenerateTokens(id, userName, principal.Claims.ToArray(), now);
        }

        public (ClaimsPrincipal claims, JwtSecurityToken token) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _identityConfig.Issuer,
                        ValidAudience = _identityConfig.Audience,
                        RequireExpirationTime = true,
                        RequireAudience = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_identityConfig.ServiceKey)),
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken ?? throw new NullReferenceException("Token равен null."));
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
