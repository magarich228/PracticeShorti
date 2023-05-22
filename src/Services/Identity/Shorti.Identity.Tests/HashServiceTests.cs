using Shorti.Identity.Api.Services;

namespace Shorti.Identity.Tests
{
    [TestClass]
    public class HashServiceTests
    {
        [TestMethod]
        public void VerifyHashPasswordTest()
        {
            HashService hashService = new HashService();

            string password = "sobaka123";

            string actual = hashService.HashPassword(password);

            var result = hashService.VerifyHashedPassword(actual, password);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VerifyHashPasswordFalseTest()
        {
            HashService hashService = new HashService();

            string password = "sobaka123";

            string actual = hashService.HashPassword(password);

            var result = hashService.VerifyHashedPassword(actual, "sobaka");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyHashInvalidArgumentsTest()
        {
            HashService hashService = new();

            Assert.ThrowsException<ArgumentNullException>(() => hashService.VerifyHashedPassword(null!, "password123"));
            Assert.ThrowsException<ArgumentNullException>(() => hashService.VerifyHashedPassword("password123", null!));
            Assert.ThrowsException<ArgumentNullException>(() => hashService.VerifyHashedPassword("", ""));
        }
    }
}

