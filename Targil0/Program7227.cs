using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome7227();
            Welcome3024();
            Console.ReadKey();
        }
        static partial void Welcome3024();
        private static void Welcome7227()
        {
            Console.Write("Enter your name:");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}
