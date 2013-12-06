using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TwoStepsAuthenticator.UnitTests {

    [TestFixture]
    public class CounterAuthenticatorTests {
        private MockUsedCodesManager mockUsedCodesManager { get; set; }

        [SetUp]
        public void SetUp() {
            this.mockUsedCodesManager = new MockUsedCodesManager();
        }

        [Test]
        public void Uses_usedCodesManager() {
            var authenticator = new CounterAuthenticator(usedCodeManager: mockUsedCodesManager);
            var secret = Authenticator.GenerateKey();
            var code = authenticator.GetCode(secret, 42uL);

            authenticator.CheckCode(secret, code, 42uL);
            Assert.AreEqual(mockUsedCodesManager.LastChallenge, 42uL);
            Assert.AreEqual(mockUsedCodesManager.LastCode, code);
        }

        [Test]
        public void CreateKey() {
            var authenticator = new CounterAuthenticator(usedCodeManager: mockUsedCodesManager);
            var secret = Authenticator.GenerateKey();
            var code = authenticator.GetCode(secret, 0uL);

            Assert.IsTrue(authenticator.CheckCode(secret, code, 0uL), "Generated Code doesn't verify");
        }

        // Test Values from http://www.ietf.org/rfc/rfc4226.txt - Appendix D
        [TestCase("12345678901234567890", 0uL, "755224")]
        [TestCase("12345678901234567890", 1uL, "287082")]
        [TestCase("12345678901234567890", 2uL, "359152")]
        [TestCase("12345678901234567890", 3uL, "969429")]
        [TestCase("12345678901234567890", 4uL, "338314")]
        [TestCase("12345678901234567890", 5uL, "254676")]
        [TestCase("12345678901234567890", 6uL, "287922")]
        [TestCase("12345678901234567890", 7uL, "162583")]
        [TestCase("12345678901234567890", 8uL, "399871")]
        [TestCase("12345678901234567890", 9uL, "520489")]
        public void VerifyKeys(string secret, ulong counter, string code) {
            var authenticator = new CounterAuthenticator(usedCodeManager: mockUsedCodesManager);
            var base32Secret = Base32Encoding.ToString(Encoding.ASCII.GetBytes(secret));

            Assert.IsTrue(authenticator.CheckCode(base32Secret, code, counter));

        }

        [Test]
        public void VerifyUsedCounter() {
            var authenticator = new CounterAuthenticator(usedCodeManager: mockUsedCodesManager);

            // Test Values from http://www.ietf.org/rfc/rfc4226.txt - Appendix D
            var base32Secret = Base32Encoding.ToString(Encoding.ASCII.GetBytes("12345678901234567890"));

            ulong usedCounter;
            Assert.True(authenticator.CheckCode(base32Secret, "520489", 0uL, out usedCounter));

            Assert.AreEqual(usedCounter, 9uL);
        }
    }
}
