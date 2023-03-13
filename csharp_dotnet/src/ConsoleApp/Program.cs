using System;
using Bcan.Playground;

namespace ConsoleApp
{
    class Program
    {
        private static void TestValueTypes()
        {
            var mp1 = new Point(3, 4);
            var mp2 = new Point(3, 4);
            //var isEqual = mp1 == mp2; compile time error - not allowed for structs
            // since operator== checks for reference equality
            var isEqual = mp1.Equals(mp2);
            Console.WriteLine($"{nameof(mp1)} == {nameof(mp2)}: {isEqual}");
            Console.WriteLine($"{nameof(mp1)} == null: {mp1.Equals(null)}");
            
        }
        static void Main(string[] args)
        {
            TestValueTypes();
        }
    }
}
