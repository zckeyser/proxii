using System.Collections.Generic;

namespace Proxii
{
    public static class Logger
    {
        private static List<string> History = new List<string>();

        public static void Log(string message)
        {
            History.Add(message);
        }

        public static IEnumerable<string> GetHistory()
        {
            foreach (var log in History)
                yield return log;
        }
    }
}
