using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class DishManager : IDishManager
    {
        /// <summary>
        /// Takes an Order object, sorts the orders and builds a list of dishes to be returned. 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Dish> GetDishes(Order order)
        {
            var returnValue = new List<Dish>();
            // Ordenar os pratos conforme a necessidade
            order.Dishes.Sort();
            // Passar o período e o tipo de prato para o método de adição
            foreach (var dishType in order.Dishes)
            {
                AddOrderToList(order.Period, dishType, returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Takes an int, representing an order type, tries to find it in the list.
        /// If the dish type does not exist, add it and set count to 1
        /// If the type exists, check if multiples are allowed and increment that instances count by one
        /// else throw error
        /// </summary>
        /// <param name="period">Period of the day (morning or evening)</param>
        /// <param name="order">int, represents a dishtype</param>
        /// <param name="returnValue">a list of dishes, - get appended to or changed </param>
        private void AddOrderToList(Period period, int order, List<Dish> returnValue)
        {
            // Agora passamos o período para GetOrderName
            string orderName = GetOrderName(period, order);

            var existingOrder = returnValue.SingleOrDefault(x => x.DishName == orderName);

            if (existingOrder == null)
            {
                // Se o prato ainda não existe na lista, adiciona-o
                returnValue.Add(new Dish
                {
                    DishName = orderName,
                    Count = 1
                });
            }
            else if (IsMultipleAllowed(period, order))
            {
                // Se múltiplos são permitidos, incrementa a contagem
                existingOrder.Count++;
            }
            else
            {
                // Se não são permitidos múltiplos, lança uma exceção
                throw new ApplicationException(string.Format("Multiple {0}(s) not allowed", orderName));
            }
        }

        private string GetOrderName(Period period, int order)
        {
            if (period == Period.morning)
            {
                switch (order)
                {
                    case 1:
                        return "egg";
                    case 2:
                        return "toast";
                    case 3:
                        return "coffee";
                    default:
                        throw new ApplicationException("Order does not exist for morning");
                }
            }
            else if (period == Period.evening)
            {
                switch (order)
                {
                    case 1:
                        return "steak";
                    case 2:
                        return "potato";
                    case 3:
                        return "wine";
                    case 4:
                        return "cake";
                    default:
                        throw new ApplicationException("Order does not exist for evening");
                }
            }
            throw new ApplicationException("Invalid time of day");
        }

        private bool IsMultipleAllowed(Period period, int order)
        {
            if (period == Period.morning && order == 3) // coffee
            {
                return true;
            }
            if (period == Period.evening && order == 2) // potato
            {
                return true;
            }
            return false;
        }
    }
}
