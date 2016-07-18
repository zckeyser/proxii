using System.Collections.Generic;

namespace Proxii.Test.Util
{
    public class Logger
    {
        private readonly List<string> History = new List<string>();

        public void Log(string message)
        {
            History.Add(message);
        }

        public IEnumerable<string> GetHistory()
        {
	        return History;
        }
    }
}
