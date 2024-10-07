using Application.Enums;
using Application.Models;
using Application.Services;
using GrosvenorDeveloper.WebApp.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApplicationTests
{
    [TestFixture]
    public class ServerTests
    {
        private Server _sut;
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            _context = new AppDbContext(options);

            SeedData(_context);

            var dishManager = new DishManager(_context);

            _sut = new Server(dishManager, _context);
        }

        private void SeedData(AppDbContext context)
        {
            context.Dishes.AddRange(
                new Dish { DishName = "egg", Count = 1 },
                new Dish { DishName = "toast", Count = 1 },
                new Dish { DishName = "coffee", Count = 1 },
                new Dish { DishName = "steak", Count = 1 },
                new Dish { DishName = "potato", Count = 1 },
                new Dish { DishName = "wine", Count = 1 },
                new Dish { DishName = "cake", Count = 1 }
            );

            context.MenuItems.AddRange(
                new MenuItem { DishId = 1, Period = Period.morning }, // egg
                new MenuItem { DishId = 2, Period = Period.morning }, // toast
                new MenuItem { DishId = 3, Period = Period.morning }, // coffee
                new MenuItem { DishId = 4, Period = Period.evening }, // steak
                new MenuItem { DishId = 5, Period = Period.evening }, // potato
                new MenuItem { DishId = 6, Period = Period.evening }, // wine
                new MenuItem { DishId = 7, Period = Period.evening }  // cake
            );

            context.SaveChanges();
        }

        [Test]
        public async Task TakeOrderFromDb_ValidMorningOrder_ReturnsCorrectDishes()
        {
            var orderInput = "morning, 1, 2, 3";

            var result = await _sut.TakeOrderFromDb(orderInput);

            Assert.AreEqual("egg,toast,coffee", result);
        }

        [Test]
        public async Task TakeOrderFromDb_ValidEveningOrder_ReturnsCorrectDishes()
        {
            var orderInput = "evening, 4, 5, 6, 7";

            var result = await _sut.TakeOrderFromDb(orderInput);

            Assert.AreEqual("steak,potato,wine,cake", result);
        }

        [Test]
        public async Task TakeOrderFromDb_InvalidPeriod_ThrowsApplicationException()
        {
            var orderInput = "invalid, 1, 2";

            var result = await _sut.TakeOrderFromDb(orderInput);

            Assert.AreEqual("Invalid time of day. Please specify 'morning' or 'evening'.", result);
        }

        [Test]
        public async Task TakeOrderFromDb_InvalidDish_ThrowsApplicationException()
        {
            var orderInput = "morning, 1, 4";

            var result = await _sut.TakeOrderFromDb(orderInput);

            Assert.AreEqual("Dish with ID 4 is not available for morning", result);
        }

        [Test]
        public void ErrorGetsReturnedWithBadInput()
        {
            var order = "morning,one"; 
            string expected = "Order needs to be a comma-separated list of numbers."; 
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CanServeSteak()
        {
            var order = "evening,1"; 
            string expected = "steak";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServe2Potatoes()
        {
            var order = "evening,2,2"; 
            string expected = "potato(x2)";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteakPotatoWineCake()
        {
            var order = "evening,1,2,3,4"; 
            string expected = "steak,potato,wine,cake";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteakPotatox2Cake()
        {
            var order = "evening,1,2,2,4"; 
            string expected = "steak,potato(x2),cake";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanGenerateErrorWithWrongDish()
        {
            var order = "evening,1,2,3,5"; 
            string expected = "Order does not exist for evening"; 
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanGenerateErrorWhenTryingToServeMoreThanOneSteak()
        {
            var order = "evening,1,1,2,3";
            string expected = "Multiple steak(s) not allowed";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }
    }
}
