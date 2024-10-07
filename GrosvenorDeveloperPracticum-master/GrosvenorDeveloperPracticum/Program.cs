using System;
using Application.Services;
using GrosvenorDeveloper.WebApp.Context;
using Microsoft.EntityFrameworkCore;

namespace GrosvenorInHousePracticum
{
    class Program
    {
        static void Main()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            var context = new AppDbContext(options);

            var dishManager = new DishManager(context);
            var server = new Server(dishManager, context);

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
                    Console.WriteLine(dishManager.SeeMorninMenuMock());
                    Console.WriteLine(dishManager.SeeMorninMenuMock());
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
