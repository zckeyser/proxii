using System;

namespace Proxii
{
    public class Foo : IFoo
    {
        public void Bar()
        {
            Console.WriteLine("Bar");
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
