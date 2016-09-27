using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiByMethodNamePatternTest
    {
        [TestMethod]
        public void Proxii_ByMethodNamePattern_NoPatterns()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .ByMethodNamePattern()
                .BeforeInvoke(() => logger.Log("foo"))
                .Create();

            proxy.IntMethod();

            var history = logger.GetHistory();
            
            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Proxii_ByMethodNamePattern_SinglePattern_Matching()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .ByMethodNamePattern("^Thr.w.*")
                .Catch<ArgumentException>(e => logger.Log("foo"))
                .Create();

            proxy.ThrowAction(new ArgumentNullException());

            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual("foo", history[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_ByMethodNamePattern_SinglePattern_NonMatching()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .ByMethodNamePattern("^Thr$")
                .Catch<ArgumentException>(e => logger.Log("foo"))
                .Create();

            proxy.ThrowAction(new ArgumentNullException());

            Assert.Fail("ThrowAction should not have been intercepted");
        }

        [TestMethod]
        public void Proxii_ByMethodNamePattern_MultiplePatterns_Matching()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .ByMethodNamePattern("^Thr.w.*")
                .Catch<ArgumentException>(e => logger.Log("foo"))
                .Create();

            proxy.ThrowAction(new ArgumentNullException());
            proxy.ThrowFunc(new ArgumentNullException(), 1);

            var history = logger.GetHistory();

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual("foo", history[0]);
            Assert.AreEqual("foo", history[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_ByMethodNamePattern_MultiplePatterns_NonMatching()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .ByMethodNamePattern("^Thr$")
                .Catch<ArgumentException>(e => logger.Log("foo"))
                .Create();

            proxy.ThrowAction(new ArgumentNullException());

            Assert.Fail("ThrowAction should not have been intercepted");
        }
    }
}
