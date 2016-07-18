using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Proxii.Test.Util.TestClasses
{
	public interface IProxiiTester
	{
		void DoAction(Action action);

		T DoFunc<T>(Func<T> func);

		void Throw(Exception e);
	}
}
