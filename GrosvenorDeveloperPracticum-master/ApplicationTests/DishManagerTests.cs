using System;
using System.Collections.Generic;
using System.Linq;
using Application.Enums;
using Application.Models;
using Application.Services;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class DishManagerTests
    {
        private DishManager _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new DishManager();
        }

        [Test]
        public void ReturnsBothMenu()
        {
            var menu1 = _sut.SeeMorninMenu();
            var menu2 = _sut.SeeEveningMenu();
            Assert.AreEqual("Mornin: | 1 - Egg | 2 -  toast | 3 - coffee |", menu1);
            Assert.AreEqual("Evening: | 1 - steak | 2 - potato  | 3 - wine  | 4 - cake |", menu2);
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
                Period = Period.evening, // Adicionado o período
                Dishes = new List<int> { 1 } // steak
            };

            var actual = _sut.GetDishes(order);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("steak", actual.First().DishName);
            Assert.AreEqual(1, actual.First().Count);
        }

        // Adicionando novos testes para os pratos da manhã
        [Test]
        public void ListWith1EggReturnsOneEgg()
        {
            var order = new Order
            {
                Period = Period.morning, // Período da manhã
                Dishes = new List<int> { 1 } // egg
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
                Dishes = new List<int> { 3 } // coffee
            };

            var actual = _sut.GetDishes(order);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("coffee", actual.First().DishName);
            Assert.AreEqual(1, actual.First().Count);
        }

        // Testes adicionais podem ser feitos para outras combinações e pratos
    }
}
