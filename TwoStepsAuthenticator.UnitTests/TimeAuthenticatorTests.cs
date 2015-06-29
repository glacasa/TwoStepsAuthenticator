using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TwoStepsAuthenticator.UnitTests
{

    [TestFixture]
    public class TimeAuthenticatorTests
    {
        private MockUsedCodesManager mockUsedCodesManager { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.mockUsedCodesManager = new MockUsedCodesManager();
        }

        [Test]
        public void CreateKey()
        {
            var authenticator = new TimeAuthenticator(mockUsedCodesManager);
            var secret = Authenticator.GenerateKey();
            var code = authenticator.GetCode(secret);

            Assert.IsTrue(authenticator.CheckCode(secret, code, "dummyuser"), "Generated Code doesn't verify");
        }

        [Test]
        public void UsesUsedCodesManager()
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var authenticator = new TimeAuthenticator(mockUsedCodesManager, () => date);
            var secret = Authenticator.GenerateKey();
            var code = authenticator.GetCode(secret);

            authenticator.CheckCode(secret, code, "dummyuser");

            Assert.AreEqual(mockUsedCodesManager.LastChallenge, 0uL);
            Assert.AreEqual(mockUsedCodesManager.LastCode, code);
        }

        [Test]
        public void PreventCodeReuse() {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var usedCodesManager = new UsedCodesManager();
            var authenticator = new TimeAuthenticator(usedCodesManager, () => date);
            var secret = Authenticator.GenerateKey();
            var code = authenticator.GetCode(secret);

            Assert.IsTrue(authenticator.CheckCode(secret, code, "dummyuser"));
            Assert.IsTrue(authenticator.CheckCode(secret, code, "otheruser"));
            Assert.IsFalse(authenticator.CheckCode(secret, code, "dummyuser"));
        }

        // Test Vectors from http://tools.ietf.org/html/rfc6238#appendix-B have all length 8. We want a length of 6.
        // This Test Vectors are from a Ruby implementation. They work with the Google Authentificator app.
        [TestCase("DRMK64PPMMC7TDZF", "2013-12-04 18:33:01 +0100", "661188")]
        [TestCase("EQOGSM3XZUH6SE2Y", "2013-12-04 18:34:56 +0100", "256804")]
        [TestCase("4VU7EQACVDMFJSBG", "2013-12-04 18:36:16 +0100", "800872")]
        public void VerifyKeys(string secret, string timeString, string code)
        {
            var date = DateTime.Parse(timeString);

            var authenticator = new TimeAuthenticator(mockUsedCodesManager, () => date);
            Assert.IsTrue(authenticator.CheckCode(secret, code));

        }

        [Test]
        public void VerifyUsedTime()
        {
            var date = DateTime.Parse("2013-12-05 17:23:50 +0100");
            var authenticator = new TimeAuthenticator(mockUsedCodesManager, () => date);

            DateTime usedTime;

            Assert.True(authenticator.CheckCode("H22Q7WAMQYFZOJ2Q", "696227", null, out usedTime));

            // 17:23:50 - 30s
            var sameDate = date.AddSeconds(-30);
            Assert.AreEqual(usedTime.Hour, sameDate.Hour);
            Assert.AreEqual(usedTime.Minute, sameDate.Minute);
            Assert.AreEqual(usedTime.Second, sameDate.Second);
        }
    }
}
