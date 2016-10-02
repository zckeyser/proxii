using Castle.DynamicProxy;

namespace Proxii.Test.Util
{
    public class LoggingInterceptor : IInterceptor
	{
		private readonly Logger _logger;

		public LoggingInterceptor(Logger logger)
		{
			_logger = logger;
		}

		public void Intercept(IInvocation invocation)
		{
			_logger.Log(invocation.Method.Name);
			invocation.Proceed();
		}
	}
}
