﻿using System;

namespace Proxii.Test.Util
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

    public class ProxiiTester : IProxiiTester
	{
        public const int IntRetVal = 10;
        public const string StringRetVal = "foo";

		public void DoAction(Action action)
		{
			action();
		}

		public T DoFunc<T>(Func<T> func)
		{
			return func();
		}

        public int IntMethod()
        {
            return IntRetVal;
        }

        public void NoOp()
        {
            // do nothing
        }

        public string StringMethod()
        {
            return StringRetVal;
        }

		public string Concat(string a, string b, string c)
		{
			return a + b + c;
		}

        public void ThrowAction(Exception e)
		{
			throw e;
		}

        public T ThrowFunc<T>(Exception e, T val)
        {
            throw e;
        }
	}
}
