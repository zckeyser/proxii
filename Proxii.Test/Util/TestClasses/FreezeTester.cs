namespace Proxii.Test.Util.TestClasses
{
    public class FreezeTester : IFreezeTester
    {
        public string MyString { get; set; }
        public int MyInt { get; set; }

        private bool _myBool;
        private double _myDouble;

        public void SetMyBool(bool b)
        {
            _myBool = b;
        }

        public bool GetMyBool()
        {
            return _myBool;
        }

        public void ChangeMyDouble(double d)
        {
            _myDouble = d;
        }

        public double GetMyDouble()
        {
            return _myDouble;
        }
    }
}
