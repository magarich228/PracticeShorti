using Microsoft.IdentityModel.Tokens;
using Shorti.Identity.Api.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

#nullable disable

namespace Shorti.Identity.Tests
{
    [TestClass]
    public class JwtIdentityManagerTests
    {
        private JwtIdentityManager _jwtIdentityManager;

        private string _username;
        private Guid _userId;

        [TestInitialize]
        public void Initialize()
        {
            _username = "magarich";
            _userId = Guid.NewGuid();

            _jwtIdentityManager = new(new Shared.Kernel.Identity.IdentityConfiguration());
        }

        [TestMethod]
        public void AllScenariosDecodeMethodTest()
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, _username),
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            };

            var jwtResult = _jwtIdentityManager.GenerateTokens(_userId, _username, claims, DateTime.UtcNow);
            var actual = _jwtIdentityManager.DecodeJwtToken(jwtResult.AccessToken);

            Assert.IsNotNull(actual);

            var actualClaimsValues = actual.claims.Claims.Select(c => c.Value).ToArray();

            foreach (var claim in claims)
            {
                CollectionAssert.Contains(actualClaimsValues, claim.Value);
            }
        }

        [TestMethod]
        public void ThrowSecurityTokenExceptionTest()
        {
            Assert.ThrowsException<SecurityTokenException>(() => _jwtIdentityManager.DecodeJwtToken(string.Empty));
        }
    }
}