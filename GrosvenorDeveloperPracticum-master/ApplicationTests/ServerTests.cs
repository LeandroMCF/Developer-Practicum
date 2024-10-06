using Application.Services;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class ServerTests
    {
        private Server _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new Server(new DishManager());
        }

        [Test]
        public void ErrorGetsReturnedWithBadInput()
        {
            var order = "morning,one"; // Input inválido
            string expected = "Order needs to be a comma-separated list of numbers."; // Mensagem esperada atualizada
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CanServeSteak()
        {
            var order = "evening,1"; // Agora especificando o período
            string expected = "steak";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServe2Potatoes()
        {
            var order = "evening,2,2"; // Adicionado o período
            string expected = "potato(x2)";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteakPotatoWineCake()
        {
            var order = "evening,1,2,3,4"; // Adicionado o período
            string expected = "steak,potato,wine,cake";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanServeSteakPotatox2Cake()
        {
            var order = "evening,1,2,2,4"; // Adicionado o período
            string expected = "steak,potato(x2),cake";
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanGenerateErrorWithWrongDish()
        {
            var order = "evening,1,2,3,5"; // Adicionado o período
            string expected = "Order does not exist for evening"; // Mensagem esperada atualizada
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanGenerateErrorWhenTryingToServeMoreThanOneSteak()
        {
            var order = "evening,1,1,2,3"; // Adicionado o período
            string expected = "Multiple steak(s) not allowed"; // Mensagem esperada atualizada
            var actual = _sut.TakeOrder(order);
            Assert.AreEqual(expected, actual);
        }
    }
}
