namespace Proxii.Test.Util.TestClasses
{
    interface IArgumentTester
    {
        void OneArg1(int a);
        void OneArg2(string a);
        void TwoArg1(int a, int b);
        void TwoArg2(string a, string b);
        void ThreeArg1(int a, int b, int c);
        void ThreeArg2(string a, string b, string c);
        void FourArg1(int a, int b, int c, int d);
        void FourArg2(string a, string b, string c, string d);
        void FiveArg1(int a, int b, int c, int d, int e);
        void FiveArg2(string a, string b, string c, string d, string e);
        void SixArg1(int a, int b, int c, int d, int e, int f);
        void SixArg2(string a, string b, string c, string d, string e, string f);
        void SevenArg1(int a, int b, int c, int d, int e, int f, int g);
        void SevenArg2(string a, string b, string c, string d, string e, string f, string g);
    }
}
