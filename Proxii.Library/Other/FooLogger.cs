using Castle.DynamicProxy;

namespace Proxii
{
    public class FooLogger
    {
        private ProxyGenerator generator = new ProxyGenerator();
        
        public IFoo Proxy(IFoo impl)
        {
            return (IFoo) generator.CreateInterfaceProxyWithTarget(typeof(IFoo), impl, new[] { new LoggingInterceptor() });
        }
    }
}
