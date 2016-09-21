namespace Proxii.Test.Util.TestClasses
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
}
