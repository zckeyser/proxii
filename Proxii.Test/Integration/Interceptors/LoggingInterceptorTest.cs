using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proxii.Test
{
    [TestClass]
    public class LoggingInterceptorTest
    {
        [TestMethod]
        public void Foo_CallsLogger()
        {
            var fooProxyGen = new FooLogger();

            var proxy = fooProxyGen.Proxy(new Foo());

            proxy.Fizz();
            proxy.Buzz();

            var logResults = Logger.GetHistory();

            var result = string.Join("", logResults);

            Assert.AreEqual("FizzBuzz", result);
        }
    }
}
