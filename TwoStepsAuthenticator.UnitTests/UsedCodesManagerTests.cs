﻿using System;
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

            Assert.IsFalse(manager.IsCodeUsed(42L, "def"));
            manager.AddCode(42L, "def");
            Assert.IsTrue(manager.IsCodeUsed(42L, "def"));
        }

    }

    internal class MockUsedCodesManager : IUsedCodesManager {
        public long? LastChallenge { get; private set; }
        public string LastCode { get; private set; }

        public void AddCode(long challenge, string code) {
            this.LastChallenge = challenge;
            this.LastCode = code;
        }

        public bool IsCodeUsed(long challenge, string code) {
            return false;
        }
    }
}
