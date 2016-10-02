using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiGroupTest
    {
        [TestMethod]
        public void Proxii_Group_SingleGroup_NoUniversal()
        {
            var logger = new Logger();
            Action<int> onReturn = n => logger.Log(n.ToString());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                              .Group(p => p.ByMethodNamePattern("^*Method$")
                                           .OnReturn(onReturn))
                              .Create();

            proxy.IntMethod();

            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(ProxiiTester.IntRetVal.ToString(), history[0]);
        }

        [TestMethod]
        public void Proxii_Group_SingleGroup_WithUniversal()
        {
            var logger = new Logger();
            Action<int> onReturn = n => logger.Log(n.ToString());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                              .Group(p => p.ByMethodNamePattern("^*Method$")
                                           .OnReturn(onReturn)) // should just get called on methods that match the pattern
                              .BeforeInvoke(method => logger.Log("calling " + method.Name)) // should get called on everything
                              .Create();

            proxy.IntMethod();
            proxy.Concat("a", "b", "c");

            var history = logger.GetHistory();

            Assert.AreEqual(3, history.Count);
            Assert.AreEqual("calling IntMethod", history[0]);
            Assert.AreEqual(ProxiiTester.IntRetVal.ToString(), history[1]);
            Assert.AreEqual("calling Concat", history[2]);
        }

        [TestMethod]
        public void Proxii_Group_TwoGroups_NoUniversal()
        {
            var logger = new Logger();
            Action<int> onReturn = n => logger.Log(n.ToString());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                              .Group(p => p.ByMethodNamePattern("^Int")
                                           .OnReturn(onReturn)) // only apply to functions starting with "Int"
                              .Group(p => p.ByReturnType(typeof(string))
                                            .ChangeReturnValue((string s) => new string(s.Reverse().ToArray()))) // only apply to functions that return a string
                              .Create();

            proxy.IntMethod();
            var result = proxy.StringMethod();
            var expected = new string(ProxiiTester.StringRetVal.Reverse().ToArray());

            Assert.AreEqual(expected, result);

            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(ProxiiTester.IntRetVal.ToString(), history[0]);
        }

        [TestMethod]
        public void Proxii_Group_TwoGroups_WithUniversal()
        {
            var logger = new Logger();
            Action<int> onReturn = n => logger.Log(n.ToString());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                              .Group(p => p.ByMethodNamePattern("^Int")
                                           .OnReturn(onReturn)) // only apply to functions starting with "Int"
                              .Group(p => p.ByReturnType(typeof(string))
                                            .ChangeReturnValue((string s) => new string(s.Reverse().ToArray()))) // only apply to functions that return a string
                              .BeforeInvoke(method => logger.Log("calling " + method.Name)) // should get called on everything
                              .Create();

            proxy.IntMethod();
            var result = proxy.StringMethod();
            var expected = new string(ProxiiTester.StringRetVal.Reverse().ToArray());

            Assert.AreEqual(expected, result);

            var history = logger.GetHistory();

            Assert.AreEqual(3, history.Count);
            Assert.AreEqual("calling IntMethod", history[0]);
            Assert.AreEqual(ProxiiTester.IntRetVal.ToString(), history[1]);
            Assert.AreEqual("calling StringMethod", history[2]);
        }

        [TestMethod]
        public void Proxii_Group_MultipleGroups_NoUniversal()
        {
            var logger = new Logger();
            Action<int> onReturn = n => logger.Log(n.ToString());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                              .Group(p => p.ByMethodNamePattern("^Int")
                                           .OnReturn(onReturn)) // only apply to functions starting with "Int"
                              .Group(p => p.ByReturnType(typeof(string))
                                            .ChangeReturnValue((string s) => new string(s.Reverse().ToArray()))) // only apply to functions that return a string
                              .Group(p => p.ByMethodName("Concat")
                                            .AfterInvoke(method => logger.Log("finished " + method.Name))) // should only apply when Concat is called
                              .Create();

            proxy.IntMethod();
            proxy.Concat("a", "b", "c");
            var result = proxy.StringMethod();
            var expected = new string(ProxiiTester.StringRetVal.Reverse().ToArray());

            Assert.AreEqual(expected, result);

            var history = logger.GetHistory();

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual(ProxiiTester.IntRetVal.ToString(), history[0]);
            Assert.AreEqual("finished Concat", history[1]);
        }

        [TestMethod]
        public void Proxii_Group_MultipleGroups_WithUniversal()
        {
            var logger = new Logger();
            Action<int> onReturn = n => logger.Log(n.ToString());

            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                              .Group(p => p.ByMethodNamePattern("^Int")
                                           .OnReturn(onReturn)) // only apply to functions starting with "Int"
                              .Group(p => p.ByReturnType(typeof(string))
                                            .ChangeReturnValue((string s) => new string(s.Reverse().ToArray()))) // only apply to functions that return a string
                              .Group(p => p.ByMethodName("Concat")
                                            .AfterInvoke(method => logger.Log("finished " + method.Name))) // should only apply when Concat is called
                              .BeforeInvoke(method => logger.Log("calling " + method.Name)) // should get called on everything
                              .Create();

            proxy.IntMethod();
            proxy.Concat("a", "b", "c");
            var result = proxy.StringMethod();
            var expected = new string(ProxiiTester.StringRetVal.Reverse().ToArray());

            Assert.AreEqual(expected, result);

            var history = logger.GetHistory();

            Assert.AreEqual(5, history.Count);
            Assert.AreEqual("calling IntMethod", history[0]);
            Assert.AreEqual(ProxiiTester.IntRetVal.ToString(), history[1]);
            Assert.AreEqual("calling Concat", history[2]);
            Assert.AreEqual("finished Concat", history[3]);
            Assert.AreEqual("calling StringMethod", history[4]);
        }
    }
}
