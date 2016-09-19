using System;

namespace Proxii.Test.Util.TestClasses
{
    public interface IArgumentTypeSelectorTester
    {
        void DoActionNoArgs(Action action);
        void DoActionStringArg(Action<string> action, string s);
        void DoActionStringIntArg(Action<string, int> action, string s, int i);
        void DoActionStringIntLongArg(Action<string, int, long> action, string s, int i, long l);
    }
}
