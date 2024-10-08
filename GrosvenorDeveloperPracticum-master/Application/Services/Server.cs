﻿using Application.Enums;
using Application.Models;
using GrosvenorDeveloper.WebApp.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Server : IServer
    {
        private readonly IDishManager _dishManager;
        private readonly AppDbContext _context;

        public Server(
            IDishManager dishManager, 
            AppDbContext context)
        {
            _dishManager = dishManager;
            _context = context;
        }

        /// <summary>
        /// This method 
        /// </summary>
        /// <param name="unparsedOrder"></param>
        /// <returns></returns>
        public async Task<string> TakeOrderFromDb(string unparsedOrder)
        {
            try
            {
                Order order = ParseOrder(unparsedOrder);

                List<Dish> dishes = await GetDishesForOrder(order);

                string returnValue = FormatOutput(dishes);

                return returnValue;
            }
            catch (ApplicationException e)
            {
                return e.Message;
            }
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

        private async Task<List<Dish>> GetDishesForOrder(Order order)
        {
            var dishList = new List<Dish>();

            foreach (var dishId in order.Dishes)
            {
                var menuItem = await _context.MenuItems
                    .Include(mi => mi.Dish)
                    .FirstOrDefaultAsync(mi => mi.DishId == dishId && mi.Period == order.Period);

                if (menuItem != null)
                {
                    dishList.Add(menuItem.Dish);
                }
                else
                {
                    throw new ApplicationException($"Dish with ID {dishId} is not available for {order.Period}");
                }
            }

            return dishList;
        }

        private Order ParseOrder(string unparsedOrder)
        {
            var returnValue = new Order
            {
                Dishes = new List<int>()
            };

            var orderItems = unparsedOrder.Split(',');

            string periodInput = orderItems[0].Trim().ToLower();

            if (!Enum.TryParse(periodInput, true, out Period period))
            {
                throw new ApplicationException("Invalid time of day. Please specify 'morning' or 'evening'.");
            }

            returnValue.Period = period;

            for (int i = 1; i < orderItems.Length; i++)
            {
                if (int.TryParse(orderItems[i].Trim(), out int parsedOrder))
                {
                    returnValue.Dishes.Add(parsedOrder);
                }
                else
                {
                    throw new ApplicationException("Order needs to be a comma-separated list of numbers."); 
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
