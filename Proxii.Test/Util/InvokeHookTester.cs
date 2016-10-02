namespace Proxii.Test.Util
{
    public interface IInvokeHookTester
    {
        int Value { get; set; }
        void SetValue(int value);
    }

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
