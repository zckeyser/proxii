namespace Proxii.Test.Util
{
    public interface IFreezeTester
    {
        string MyString { get; set; }
        int MyInt { get; set; }

        void SetMyBool(bool b);
        bool GetMyBool();

        void ChangeMyDouble(double d);
        double GetMyDouble();
    }

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
