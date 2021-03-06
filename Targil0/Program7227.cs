using System;

namespace Targil0
{
    partial class Program
    {

        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }

            public override string ToString()
            {
                return $"Customer name: {Name}\n" +
                    $"Id: {Id}\n" +
                    $"Phone: {Phone}\n" +
                    $"Longitude: {Longitude}\n" +
                    $"Lattitude: {Lattitude}\n";
            }
        }


        static void Main(string[] args)
        {



            Customer elyasaf = new Customer();
            Console.WriteLine(elyasaf.ToString());


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
