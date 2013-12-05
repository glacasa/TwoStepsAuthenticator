using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TwoStepsAuthenticator.UnitTests {

    [TestFixture]
    public class CounterAuthenticatorTests {

        [Test]
        public void CreateKey() {
            var authenticator = new CounterAuthenticator();
            var secret = Authenticator.GenerateKey();
            var code = authenticator.GetCode(secret, 0L);

            Assert.IsTrue(authenticator.CheckCode(secret, code, 0L), "Generated Code doesn't verify");
        }

        // Test Values from http://www.ietf.org/rfc/rfc4226.txt - Appendix D
        [TestCase("12345678901234567890", 0L, "755224")]
        [TestCase("12345678901234567890", 1L, "287082")]
        [TestCase("12345678901234567890", 2L, "359152")]
        [TestCase("12345678901234567890", 3L, "969429")]
        [TestCase("12345678901234567890", 4L, "338314")]
        [TestCase("12345678901234567890", 5L, "254676")]
        [TestCase("12345678901234567890", 6L, "287922")]
        [TestCase("12345678901234567890", 7L, "162583")]
        [TestCase("12345678901234567890", 8L, "399871")]
        [TestCase("12345678901234567890", 9L, "520489")]
        public void VerifyKeys(string secret, long counter, string code) {
            var authenticator = new CounterAuthenticator();
            var base32Secret = Base32Encoding.ToString(Encoding.ASCII.GetBytes(secret));

            Assert.IsTrue(authenticator.CheckCode(base32Secret, code, counter));

        }

    }
}
