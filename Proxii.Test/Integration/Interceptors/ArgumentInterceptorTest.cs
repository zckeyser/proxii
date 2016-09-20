using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class ArgumentInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void Integration_ArgumentInterceptor_1Arg_MatchingSignature()
        {
            Func<int, int> modifier = (a) => a + 1;

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int>(modifier) };
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_1Arg_NonMatchingSignature()
        {
            Assert.Fail();
        }
    }
}
