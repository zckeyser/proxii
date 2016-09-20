using System;

namespace Proxii.Test.Util.TestClasses
{
    public class ArgumentTester : IArgumentTester
    {
        public Logger Logger { get; set; }

        public void OneArg1(int a)
        {
            Logger.Log(string.Format("{0}", a));
        }

        public void OneArg2(string a)
        {
            Logger.Log(string.Format("{0}", a));
        }

        public void TwoArg1(int a, int b)
        {
            Logger.Log(string.Format("{0}{1}", a, b));
        }

        public void TwoArg2(string a, string b)
        {
            Logger.Log(string.Format("{0}{1}", a, b));
        }

        public void ThreeArg1(int a, int b, int c)
        {
            Logger.Log(string.Format("{0}{1}{2}", a, b, c));
        }

        public void ThreeArg2(string a, string b, string c)
        {
            Logger.Log(string.Format("{0}{1}{2}", a, b, c));
        }

        public void FourArg1(int a, int b, int c, int d)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}", a, b, c, d));
        }

        public void FourArg2(string a, string b, string c, string d)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}", a, b, c, d));
        }

        public void FiveArg1(int a, int b, int c, int d, int e)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}{4}", a, b, c, d, e));
        }

        public void FiveArg2(string a, string b, string c, string d, string e)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}{4}", a, b, c, d, e));
        }

        public void SixArg1(int a, int b, int c, int d, int e, int f)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}{4}{5}", a, b, c, d, e, f));
        }

        public void SixArg2(string a, string b, string c, string d, string e, string f)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}{4}{5}", a, b, c, d, e, f));
        }

        public void SevenArg1(int a, int b, int c, int d, int e, int f, int g)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}{4}{5}{6}", a, b, c, d, e, f, g));
        }

        public void SevenArg2(string a, string b, string c, string d, string e, string f, string g)
        {
            Logger.Log(string.Format("{0}{1}{2}{3}{4}{5}{6}", a, b, c, d, e, f, g));
        }
    }
}
