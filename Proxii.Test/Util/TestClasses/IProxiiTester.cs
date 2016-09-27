using System;

namespace Proxii.Test.Util.TestClasses
{
	public interface IProxiiTester
	{
		void DoAction(Action action);

		T DoFunc<T>(Func<T> func);

		void ThrowAction(Exception e);

        T ThrowFunc<T>(Exception e, T val);

        string StringMethod();

        int IntMethod();

		string Concat(string a, string b, string c);

        void NoOp();
	}
}
