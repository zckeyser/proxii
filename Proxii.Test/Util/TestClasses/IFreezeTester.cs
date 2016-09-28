using System.Security.Cryptography.X509Certificates;

namespace Proxii.Test.Util.TestClasses
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
}
