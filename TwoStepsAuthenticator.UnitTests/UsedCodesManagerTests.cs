using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TwoStepsAuthenticator.UnitTests {
    
    [TestFixture]
    public class UsedCodesManagerTests {

        [Test]
        public void Can_add_codes() {
            var manager = new UsedCodesManager();

            Assert.IsFalse(manager.IsCodeUsed(42uL, "def"));
            manager.AddCode(42uL, "def");
            Assert.IsTrue(manager.IsCodeUsed(42uL, "def"));
        }

    }

    internal class MockUsedCodesManager : IUsedCodesManager {
        public ulong? LastChallenge { get; private set; }
        public string LastCode { get; private set; }

        public void AddCode(ulong challenge, string code) {
            this.LastChallenge = challenge;
            this.LastCode = code;
        }

        public bool IsCodeUsed(ulong challenge, string code) {
            return false;
        }
    }
}
