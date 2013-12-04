using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TwoStepsAuthenticator.UnitTests {
    
    [TestFixture]
    public class AuthenticatorTests {

        [Test]
        public void CreateKey() {
            var authenticator = new Authenticator();
            var secret = authenticator.GenerateKey();
            var code = authenticator.GetCode(secret);

            Assert.IsTrue(authenticator.CheckCode(secret, code), "Generated Code doesn't verify");
        }

        // Test Vectors from http://tools.ietf.org/html/rfc6238#appendix-B have all length 8. We want a length of 6.
        // This Test Vectors are from a Ruby implementation. They work with the Google Authentificator app.
        [TestCase("DRMK64PPMMC7TDZF", "2013-12-04 18:33:01 +0100", "661188")]
        [TestCase("EQOGSM3XZUH6SE2Y", "2013-12-04 18:34:56 +0100", "256804")]
        [TestCase("4VU7EQACVDMFJSBG", "2013-12-04 18:36:16 +0100", "800872")]
        public void VerifyKeys(string secret, string timeString, string code) {
            var date = DateTime.Parse(timeString);

            var authenticator = new Authenticator(() => date);
            Assert.IsTrue(authenticator.CheckCode(secret, code));

        }
    }
}
