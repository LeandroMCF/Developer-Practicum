using System;
using System.Collections.Generic;
using System.Linq;
using Application.Enums;
using Application.Models;
using Application.Services;
using GrosvenorDeveloper.WebApp.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class DishManagerTests
    {
        private DishManager _sut;
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            _context = new AppDbContext(options);

            SeedData(_context);

            _sut = new DishManager(_context);
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
        public void ReturnsBothMenu()
        {
            var menu1 = _sut.SeeMorninMenuMock();
            var menu2 = _sut.SeeEveningMenuMock();
            Assert.AreEqual("mornin: | 1 - egg | 2 -  toast | 3 - coffee |", menu1.ToLower());
            Assert.AreEqual("evening: | 1 - steak | 2 - potato  | 3 - wine  | 4 - cake |", menu2.ToLower());
        }

        [Test]
        public void EmptyListReturnsEmptyList()
        {
            var order = new Order();
            var actual = _sut.GetDishes(order);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void ListWith1SteakReturnsOneSteak()
        {
            var order = new Order
            {
                Period = Period.evening,
                Dishes = new List<int> { 1 }
            };

            var actual = _sut.GetDishes(order);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("steak", actual.First().DishName);
            Assert.AreEqual(1, actual.First().Count);
        }

        [Test]
        public void ListWith1EggReturnsOneEgg()
        {
            var order = new Order
            {
                Period = Period.morning,
                Dishes = new List<int> { 1 }
            };

            var actual = _sut.GetDishes(order);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("egg", actual.First().DishName);
            Assert.AreEqual(1, actual.First().Count);
        }

        [Test]
        public void ListWithCoffeeReturnsCoffee()
        {
            var order = new Order
            {
                Period = Period.morning,
                Dishes = new List<int> { 3 }
            };

            var actual = _sut.GetDishes(order);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("coffee", actual.First().DishName);
            Assert.AreEqual(1, actual.First().Count);
        }
    }
}
