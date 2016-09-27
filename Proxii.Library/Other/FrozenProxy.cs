using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxii.Library.Other
{
    /// <summary>
    /// Creates a proxy of the given object
    /// which blocks all set_ methods,
    /// such as those used by properties
    /// </summary>
    public static class FrozenProxy
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static T Freeze<T>(T obj)
        {
            // TODO add freezing interceptor

            // TODO proxy with interceptor and return that
            // don't forget to add a this interceptro
            return obj;
        }
    }
}
