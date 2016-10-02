using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiByArgumentTypeTest
    {
        [TestMethod]
        public void Proxii_ByArgumentType_SingleFilter_Matches()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IArgumentTypeSelectorTester, ArgumentTypeSelectorTester>()
                              .Catch<ArgumentException>((e) => logger.Log("throw"))
                              .ByArgumentType(typeof(Action<string>), typeof(string))
                              .Create();

            proxy.DoActionStringArg(
                (s) =>
                {
                    throw new ArgumentException();
                }, 
                "foo");

            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual("throw", history[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Proxii_ByArgumentType_SingleFilter_DoesNotMatch()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IArgumentTypeSelectorTester, ArgumentTypeSelectorTester>()
                              .Catch<ArgumentOutOfRangeException>((e) => logger.Log("throw"))
                              .ByArgumentType(typeof(Action<string>), typeof(string))
                              .Create();

            proxy.DoActionStringArg(
                (s) =>
                {
                    throw new ArgumentException();
                },
                "foo");

            Assert.Fail("Should have thrown ArgumentException");
        }

        [TestMethod]
        public void Proxii_ByArgumentType_MultipleFilters_Matches()
        {
            var logger = new Logger();

            var proxy = Proxii.Proxy<IArgumentTypeSelectorTester, ArgumentTypeSelectorTester>()
                              .Catch<ArgumentException>((e) => logger.Log("throw"))
                              .ByArgumentType(typeof(Action<string, int>), typeof(string), typeof(int))
                              .ByArgumentType(typeof(Action<string>), typeof(string))
                              .Create();

            proxy.DoActionStringArg(
                (s) =>
                {
                    throw new ArgumentException();
                },
                "foo"
            );

            proxy.DoActionStringIntArg(
                (s, i) =>
                {
                    throw new ArgumentException();
                },
                "foo",
                1
            );

            var history = logger.GetHistory();

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual("throw", history[0]);
            Assert.AreEqual("throw", history[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Proxii_ByArgumentType_MultipleFilters_DoesNotMatch()
        {
            var logger = new Logger();

           var proxy = Proxii.Proxy<IArgumentTypeSelectorTester, ArgumentTypeSelectorTester>()
                              .Catch<ArgumentException>((e) => logger.Log("throw"))
                              .ByArgumentType(typeof(Action<string>), typeof(string))
                              .Create();

            proxy.DoActionStringArg(
                (s) =>
                {
                    throw new ArgumentException();
                },
                "foo"
            );

            proxy.DoActionStringIntArg(
                (s, i) =>
                {
                    throw new ArgumentException();
                },
                "foo",
                1
            );

            Assert.Fail("Should have thrown ArgumentException");
        }
    }
}
