using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiOnReturnTest
    {
        [TestMethod]
        public void Proxii_OnReturn_ReturnValueOnly_Matching()
        {
            var logger = new Logger();
            Action<string> onReturn = (s) => logger.Log(s);

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.StringMethod();
            const string expected = ProxiiTester.StringRetVal;
            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueOnly_NonMatching()
        {
            var logger = new Logger();
            Action<string> onReturn = (s) => logger.Log(s);

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.IntMethod();
            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueMethodInfo_Matching()
        {
            var logger = new Logger();
            Action<string, MethodInfo> onReturn = (s, method) => logger.Log(s + " returned from " + method.Name);

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.StringMethod();
            const string expected = ProxiiTester.StringRetVal + " returned from StringMethod";
            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueMethodInfo_NonMatching()
        {
            var logger = new Logger();
            Action<string, MethodInfo> onReturn = (s, method) => logger.Log(s + " returned from " + method.Name);

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.IntMethod();
            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueArgs_Matching()
        {
            var logger = new Logger();
            Action<string, object[]> onReturn = (s, args) => logger.Log(s + " returned when called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.Concat("foo", "bar", "buzz");
            const string expected = "foobarbuzz returned when called with foo bar buzz";
            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueArgs_NonMatching()
        {
            var logger = new Logger();
            Action<string, object[]> onReturn = (s, args) => logger.Log(s + " returned when called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.IntMethod();
            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueMethodInfoArgs_Matching()
        {
            var logger = new Logger();
            Action<string, MethodInfo, object[]> onReturn = (s, method, args) => logger.Log(s + " returned when " + method.Name + " was called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.Concat("foo", "bar", "buzz");
            const string expected = "foobarbuzz returned when Concat was called with foo bar buzz";
            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
        }

        [TestMethod]
        public void Proxii_OnReturn_ReturnValueMethodInfoArgs_NonMatching()
        {
            var logger = new Logger();
            Action<string, MethodInfo, object[]> onReturn = (s, method, args) => logger.Log(s + " returned when " + method.Name + " was called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .OnReturn(onReturn)
                            .Create();

            proxy.IntMethod();
            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }
    }
}
