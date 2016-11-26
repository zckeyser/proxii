using System;

namespace Proxii.Test.Util
{
    public interface IDefaultValueTester
    {
        string DoStuff(Func<string, string> func, string s = null);
        string DoOtherStuff(Func<string, string> func, string ss = null);
    }

    public class DefaultValueTester : IDefaultValueTester
    {
        public string DoStuff(Func<string, string> func, string s = null)
        {
            return func(s);
        }

        public string DoOtherStuff(Func<string, string> func, string ss = null)
        {
            return func(ss);
        }
    }
}
