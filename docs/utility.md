# Utility Methods
The methods in this section aren't directly related to the primary Proxii functionality, but they instead provide useful behavior using the same methodology as the rest of Proxii behind the scenes.

## Freeze<T>(T obj, params string[] patterns)
Freezes the given object, such that Property set methods no longer change the object. When given strings as parameters after the object, Freeze will also block the execution of any of those methods. Use these to block the execution of any other methods that affect internal state that you'd like to present
```csharp
interface IFoo
{
    public string MyString { get; set; }
    public int MyInt { get; set; }

    public void SetMyBool();
    public bool GetMyBool();
    public void ChangeMyDouble();
    public double GetMyDouble();
}

// Foo : IFoo
var foo = new Foo();

foo.MyString = "foo";
foo.MyInt = 10;
foo.SetMyBool(true);
foo.ChangeMyDouble(5.5);

foo = Proxii.Freeze(foo, "^Set.*", "^Change.*");

foo.MyString = "bar";
foo.MyInt = 20;
foo.SetMyBool(false);
foo.ChangeMyDouble(11.0);

foo.MyString == "foo";
foo.MyInt == 10;
foo.GetMyBool(false); // would be true if we didn't pass the "^Set.*" arg to Freeze
foo.GetMyDouble(5.5); // would be 5.5 if we didn't pass the "^Change.*" arg to Freeze
```
