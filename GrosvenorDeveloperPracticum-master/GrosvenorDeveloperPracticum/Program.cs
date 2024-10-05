using System;
using Application;

namespace GrosvenorInHousePracticum
{
    class Program
    {
        static void Main()
        {
            var server = new Server(new DishManager());

            Console.WriteLine("Welcome! Please enter your order in the following format:");
            Console.WriteLine("Format: period, dishType1, dishType2, ..., dishTypeN");
            Console.WriteLine("Example (Morning): morning, 1, 2, 3");
            Console.WriteLine("Example (Evening): evening, 1, 2, 4");

            while (true)
            {
                var unparsedOrder = Console.ReadLine();
                var output = server.TakeOrder(unparsedOrder);
                Console.WriteLine(output);
            }
        }
    }
}
