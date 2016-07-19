namespace Proxii.Test.Util.TestClasses
{
	public class InvokeHookTester : IInvokeHookTester
	{
		public const int DefaultValue = 5;

		public InvokeHookTester()
		{
			Value = DefaultValue;
		}

		public int Value { get; set; }

		public void SetValue(int value)
		{
			Value = value;
		}
	}
}
