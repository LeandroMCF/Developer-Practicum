using Application.Enums;
using System;
using System.Collections.Generic;

namespace Application
{
    public class Server : IServer
    {
        private readonly IDishManager _dishManager;

        public Server(IDishManager dishManager)
        {
            _dishManager = dishManager;
        }

        public string TakeOrder(string unparsedOrder)
        {
            try
            {
                Order order = ParseOrder(unparsedOrder);
                List<Dish> dishes = _dishManager.GetDishes(order);
                string returnValue = FormatOutput(dishes);
                return returnValue;
            }
            catch (ApplicationException e)
            {
                return e.Message;
            }
        }

        private Order ParseOrder(string unparsedOrder)
        {
            var returnValue = new Order
            {
                Dishes = new List<int>()
            };

            var orderItems = unparsedOrder.Split(',');

            // Verifica o primeiro item como o período
            string periodInput = orderItems[0].Trim().ToLower();

            if (!Enum.TryParse(periodInput, true, out Period period))
            {
                throw new ApplicationException("Invalid time of day. Please specify 'morning' or 'evening'.");
            }

            returnValue.Period = period;

            // Processa os itens restantes como números
            for (int i = 1; i < orderItems.Length; i++)
            {
                if (int.TryParse(orderItems[i].Trim(), out int parsedOrder))
                {
                    returnValue.Dishes.Add(parsedOrder);
                }
                else
                {
                    throw new ApplicationException("Order needs to be a comma-separated list of numbers."); // Mensagem atualizada
                }
            }

            return returnValue;
        }

        private string FormatOutput(List<Dish> dishes)
        {
            var returnValue = "";

            foreach (var dish in dishes)
            {
                returnValue = returnValue + string.Format(",{0}{1}", dish.DishName, GetMultiple(dish.Count));
            }

            if (returnValue.StartsWith(","))
            {
                returnValue = returnValue.TrimStart(',');
            }

            return returnValue;
        }

        private object GetMultiple(int count)
        {
            if (count > 1)
            {
                return string.Format("(x{0})", count);
            }
            return "";
        }
    }
}
