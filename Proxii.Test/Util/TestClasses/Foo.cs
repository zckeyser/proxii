using System;

namespace Proxii.Test.Util
{
    public class Foo : IFoo
    {
        public string Bar()
        {
	        return "Bar";
        }

        public void Bazz()
        {
            Console.WriteLine("Bazz");
        }

        public void Buzz()
        {
            Console.WriteLine("Buzz");
        }

        public void Fizz()
        {
            Console.WriteLine("Fizz");
        }
    }
}
