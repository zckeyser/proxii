using System;

namespace Proxii.Test.Util.TestClasses
{
    public class ArgumentTypeSelectorTester : IArgumentTypeSelectorTester
    {
        public void DoActionNoArgs(Action action)
        {
            action();
        }

        public void DoActionStringArg(Action<string> action, string s)
        {
            action(s);
        }

        public void DoActionStringIntArg(Action<string, int> action, string s, int i)
        {
            action(s, i);
        }

        public void DoActionStringIntLongArg(Action<string, int, long> action, string s, int i, long l)
        {
            action(s, i, l);
        }
    }
}
