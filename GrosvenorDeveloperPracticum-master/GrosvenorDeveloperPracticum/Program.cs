using System;
using Application.Services;

namespace GrosvenorInHousePracticum
{
    class Program
    {
        static void Main()
        {
            var server = new Server(new DishManager());
            var dish = new DishManager();

            Console.WriteLine("Welcome! Please enter your order in the following format:");
            Console.WriteLine("Format: period, dishType1, dishType2, ..., dishTypeN");
            Console.WriteLine("Example (Morning): morning, 1, 2, 3");
            Console.WriteLine("Example (Evening): evening, 1, 2, 4");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("M - To see menu");
            Console.WriteLine("-----------------------------------");

            while (true)
            {
                var unparsedOrder = Console.ReadLine();
                if (unparsedOrder.ToLower() == "m")
                {
                    Console.WriteLine(dish.SeeMorninMenu());
                    Console.WriteLine(dish.SeeEveningMenu());
                    Console.WriteLine("-----------------------------------");
                }
                else
                {
                    var output = server.TakeOrder(unparsedOrder);
                    Console.WriteLine(output);
                }
            }
        }
    }
}
