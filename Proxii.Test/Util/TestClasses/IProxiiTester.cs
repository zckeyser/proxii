using System;

namespace Proxii.Test.Util.TestClasses
{
	public interface IProxiiTester
	{
		void DoAction(Action action);

		T DoFunc<T>(Func<T> func);

		void Throw(Exception e);

        string StringMethod();

        int IntMethod();

        void NoOp();
	}
}
