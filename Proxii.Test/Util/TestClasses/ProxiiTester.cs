using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxii.Test.Util.TestClasses
{
	public class ProxiiTester : IProxiiTester
	{
		public void DoAction(Action action)
		{
			action();
		}

		public T DoFunc<T>(Func<T> func)
		{
			return func();
		}

		public void Throw(Exception e)
		{
			throw e;
		}
	}
}
