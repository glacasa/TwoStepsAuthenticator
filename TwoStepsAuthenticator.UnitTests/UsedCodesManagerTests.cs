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

            Assert.IsFalse(manager.IsCodeUsed("abc", "def"));
            manager.AddCode("abc", "def");
            Assert.IsTrue(manager.IsCodeUsed("abc", "def"));
        }

    }
}
