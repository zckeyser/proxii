using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Library.Selectors;
using Proxii.Test.Util.TestClasses;
using System;

namespace Proxii.Test.Unit.Selectors
{
    [TestClass]
    public class ReturnTypeSelectorTest
    {
        [TestMethod]
        public void Unit_ReturnTypeSelector_SingleType_Matches()
        {
            var selector = new ReturnTypeSelector(typeof(int));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()) };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(LoggingInterceptor));
        }

        [TestMethod]
        public void Unit_ReturnTypeSelector_SingleType_DoesNotMatch()
        {
            var selector = new ReturnTypeSelector(typeof(string));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()) };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Unit_ReturnTypeSelector_MultipleTypes_Matches()
        {
            var selector = new ReturnTypeSelector(typeof(int), typeof(string));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()) };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(LoggingInterceptor));
        }

        [TestMethod]
        public void Unit_ReturnTypeSelector_MultipleTypes_DoesNotMatch()
        {
            var selector = new ReturnTypeSelector(typeof(string), typeof(void));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()) };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Unit_ReturnTypeSelector_MultipleInterceptors_Matches()
        {
            var selector = new ReturnTypeSelector(typeof(int));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()), new ExceptionInterceptor() };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);

            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(LoggingInterceptor));
            Assert.IsInstanceOfType(result[1], typeof(ExceptionInterceptor));
        }

        [TestMethod]
        public void Unit_ReturnTypeSelector_MultipleInterceptors_DoesNotMatch()
        {
            var selector = new ReturnTypeSelector(typeof(string));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()), new ExceptionInterceptor() };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Unit_ReturnTypeSelector_MultipleTypes_MultipleMethods_AllMatching()
        {
            // we want to make sure that this works for various types, since the previous tests were all based on int
            var selector = new ReturnTypeSelector(typeof(int), typeof(string), typeof(void));
            var interceptors = new IInterceptor[] { new LoggingInterceptor(new Logger()) };

            var result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("IntMethod"), interceptors);
            result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("StringMethod"), result);
            result = selector.SelectInterceptors(typeof(ReturnTypeSelectorTester), typeof(ReturnTypeSelectorTester).GetMethod("VoidMethod"), result);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(LoggingInterceptor));
        }
    }

    internal class ReturnTypeSelectorTester
    {
        public int IntMethod()
        {
            return 10;
        }

        public string StringMethod()
        {
            return "foo";
        }

        public void VoidMethod()
        {
            Console.WriteLine("foo");
        }
    }
}
