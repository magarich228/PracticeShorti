using Shorti.Identity.Api.Identity.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

#nullable disable

namespace Shorti.Identity.Tests
{
    [TestClass]
    public class ClaimsExtensionsTests
    {
        private Claim[] _claims;

        private string _userName;
        private Guid _userId;

        [TestInitialize]
        public void Initialize()
        {
            _userName = "emoRussianGirl";
            _userId = Guid.NewGuid();

            _claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, _userName),
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            };
        }

        [TestMethod]
        public void GetIdTest()
        {
            var actualId = _claims.GetId();

            Assert.IsNotNull(actualId);
            Assert.AreEqual(_userId, actualId);
        }

        [TestMethod]
        public void GetUserNameTest()
        {
            var actualUserName = _claims.GetUsername();

            Assert.IsTrue(!string.IsNullOrEmpty(actualUserName));
            Assert.AreEqual(_userName, actualUserName);
        }

        [TestMethod]
        public void ThrowsExceptionTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => (new Claim[] {}).GetId());
            Assert.ThrowsException<InvalidOperationException>(() => (new Claim[] {}).GetUsername());
        }
    }
}
