using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

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
            
        }

        [TestMethod]
        public void Proxii_Group_TwoGroups_NoUniversal()
        {

        }

        [TestMethod]
        public void Proxii_Group_TwoGroups_WithUniversal()
        {

        }

        [TestMethod]
        public void Proxii_Group_MultipleGroups_NoUniversal()
        {

        }

        [TestMethod]
        public void Proxii_Group_MultipleGroups_WithUniversal()
        {

        }
    }
}
