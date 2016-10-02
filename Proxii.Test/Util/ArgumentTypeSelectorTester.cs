using System;

namespace Proxii.Test.Util
{
    public interface IArgumentTypeSelectorTester
    {
        void DoActionNoArgs(Action action);
        void DoActionStringArg(Action<string> action, string s);
        void DoActionStringIntArg(Action<string, int> action, string s, int i);
        void DoActionStringIntLongArg(Action<string, int, long> action, string s, int i, long l);
    }

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
