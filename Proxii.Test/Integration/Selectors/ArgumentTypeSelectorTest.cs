using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Proxii.Internal.Selectors;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Selectors
{
    [TestClass]
    public class ArgumentTypeSelectorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void Integration_ArgumentTypeSelector_NoFilter()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");

            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Integration_ArgumentTypeSelector_OneFilter_Matches()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            selector.AddArgumentDefinition( new[] { typeof(Action<string>), typeof(string) } );
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");

            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual("DoActionStringArg", history[0]);
        }

        [TestMethod]
        public void Integration_ArgumentTypeSelector_OneFilter_DoesNotMatch()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            selector.AddArgumentDefinition(new[] { typeof(Func<bool>), typeof(string) });
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");

            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Integration_ArgumentTypeSelector_MultipleFilters_Matches()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            selector.AddArgumentDefinition(new[] { typeof(Action<string>), typeof(string) });
            selector.AddArgumentDefinition(new[] { typeof(Action<string, int>), typeof(string), typeof(int) });
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");
            proxy.DoActionStringIntArg((s, i) => Console.Write(s + i), "foo", 1);

            var history = logger.GetHistory();

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual("DoActionStringArg", history[0]);
            Assert.AreEqual("DoActionStringIntArg", history[1]);
        }

        [TestMethod]
        public void Integration_ArgumentTypeSelector_MultipleFilters_DoesNotMatch()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            selector.AddArgumentDefinition(new[] { typeof(Func<string>), typeof(string) });
            selector.AddArgumentDefinition(new[] { typeof(Func<decimal>), typeof(string), typeof(int) });
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");
            proxy.DoActionStringIntArg((s, i) => Console.Write(s + i), "foo", 1);

            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }

        [TestMethod]
        public void Integration_ArgumentTypeSelector_MultipleInterceptors_Matches()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger), new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            selector.AddArgumentDefinition(new[] { typeof(Action<string>), typeof(string) });
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");

            var history = logger.GetHistory();

            // logging is doubled since we have 2 LoggingInterceptors with the same logger
            Assert.AreEqual(2, history.Count);
            Assert.AreEqual("DoActionStringArg", history[0]);
            Assert.AreEqual("DoActionStringArg", history[1]);
        }

        [TestMethod]
        public void Integration_ArgumentTypeSelector_MultipleInterceptors_DoesNotMatch()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger), new LoggingInterceptor(logger) };
            var selector = new ArgumentTypeSelector();
            selector.AddArgumentDefinition(new[] { typeof(Func<bool>), typeof(string) });
            var options = new ProxyGenerationOptions { Selector = selector };
            var proxy = (IArgumentTypeSelectorTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTypeSelectorTester), new ArgumentTypeSelectorTester(), options, interceptors);

            proxy.DoActionStringArg((s) => Console.Write(s), "foo");

            var history = logger.GetHistory();

            Assert.AreEqual(0, history.Count);
        }
    }
}
