using System.Collections.Generic;

namespace Proxii.Test.Util.TestClasses
{
	public class Logger
	{
		private List<string> History { get; set; }

		public Logger()
		{
			History = new List<string>();
		}

		public void Log(string message)
		{
			History.Add(message);
		}

		public List<string> GetHistory()
		{
			return History;
		}
	}
}
