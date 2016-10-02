namespace Proxii.Test.Util
{
    public interface IArgumentTester
    {
        int OneArg1(int a);
        string OneArg2(string a);
        int TwoArg1(int a, int b);
        string TwoArg2(string a, string b);
        int ThreeArg1(int a, int b, int c);
        string ThreeArg2(string a, string b, string c);
        int FourArg1(int a, int b, int c, int d);
        string FourArg2(string a, string b, string c, string d);
        int FiveArg1(int a, int b, int c, int d, int e);
        string FiveArg2(string a, string b, string c, string d, string e);
        int SixArg1(int a, int b, int c, int d, int e, int f);
        string SixArg2(string a, string b, string c, string d, string e, string f);
        int SevenArg1(int a, int b, int c, int d, int e, int f, int g);
        string SevenArg2(string a, string b, string c, string d, string e, string f, string g);
    }

    public class ArgumentTester : IArgumentTester
    {
        public int OneArg1(int a)
        {
            return a;
        }

        public string OneArg2(string a)
        {
            return a;
        }

        public int TwoArg1(int a, int b)
        {
            return a + b;
        }

        public string TwoArg2(string a, string b)
        {
            return a + b;
        }

        public int ThreeArg1(int a, int b, int c)
        {
            return a + b + c;
        }

        public string ThreeArg2(string a, string b, string c)
        {
            return a + b + c;
        }

        public int FourArg1(int a, int b, int c, int d)
        {
            return a + b + c + d;
        }

        public string FourArg2(string a, string b, string c, string d)
        {
            return a + b + c + d;
        }

        public int FiveArg1(int a, int b, int c, int d, int e)
        {
            return a + b + c + d + e;
        }

        public string FiveArg2(string a, string b, string c, string d, string e)
        {
            return a + b + c + d + e;
        }

        public int SixArg1(int a, int b, int c, int d, int e, int f)
        {
            return a + b + c + d + e + f;
        }

        public string SixArg2(string a, string b, string c, string d, string e, string f)
        {
            return a + b + c + d + e + f;
        }

        public int SevenArg1(int a, int b, int c, int d, int e, int f, int g)
        {
            return a + b + c + d + e + f + g;
        }

        public string SevenArg2(string a, string b, string c, string d, string e, string f, string g)
        {
            return a + b + c + d + e + f + g;
        }
    }
}
