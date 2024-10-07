using Application.Enums;
using Application.Inputs;
using Application.Models;
using GrosvenorDeveloper.WebApp.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DishManager : IDishManager
    {
        private readonly AppDbContext _context;

        public DishManager(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes the full list of available items from the menu
        /// </summary>
        public async Task<object> GetFullMenu()
        {
            var morningMenu = await _context.MenuItems
                .Where(mi => mi.Period == Period.morning)
                .Select(mi => new { mi.Dish.Id, mi.Dish.DishName })
                .ToListAsync();

            var eveningMenu = await _context.MenuItems
                .Where(mi => mi.Period == Period.evening)
                .Select(mi => new { mi.Dish.Id, mi.Dish.DishName })
                .ToListAsync();

            var menu = new
            {
                Morning = morningMenu,
                Evening = eveningMenu
            };

            return menu;
        }

        /// <summary>
        /// Create new item in database
        /// </summary>
        /// <param name="newDish"></param>
        /// <exception cref="ApplicationException"></exception>
        public async Task AddDishToMenu(AddDish newDish)
        {
            try
            {
                if (!Enum.TryParse(newDish.time, true, out Period period))
                {
                    throw new ApplicationException("Invalid time of day. Please specify 'morning' or 'evening'.");
                }

                var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.DishName == newDish.dishName);

                if (dish == null)
                {
                    dish = new Dish
                    {
                        DishName = newDish.dishName,
                        Count = newDish.count
                    };
                    await _context.Dishes.AddAsync(dish);
                    await _context.SaveChangesAsync();
                }

                var menuItem = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.DishId == dish.Id && mi.Period == period);

                if (menuItem == null)
                {
                    var newMenuItem = new MenuItem
                    {
                        DishId = dish.Id,
                        Period = period
                    };
                    await _context.MenuItems.AddAsync(newMenuItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ApplicationException("This dish is already associated with this period.");
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Returns the avalieble dishs in menu at mornin period
        /// </summary>
        public string SeeMorninMenuMock()
        {
            try
            {
                return "Mornin: | 1 - egg | 2 -  toast | 3 - coffee |";
            }
            catch (ApplicationException e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Returns the avalieble dishs in menu at evening period
        /// </summary>
        public string SeeEveningMenuMock()
        {
            try
            {
                return "Evening: | 1 - steak | 2 - potato  | 3 - wine  | 4 - cake |";
            }
            catch (ApplicationException e)
            {
                return e.Message;
            }
        }

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
            string orderName = GetOrderName(period, order);

            var existingOrder = returnValue.SingleOrDefault(x => x.DishName == orderName);

            if (existingOrder == null)
            {
                returnValue.Add(new Dish
                {
                    DishName = orderName,
                    Count = 1
                });
            }
            else if (IsMultipleAllowed(period, order))
            {
                existingOrder.Count++;
            }
            else
            {
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
            if (period == Period.morning && order == 3)
            {
                return true;
            }
            if (period == Period.evening && order == 2)
            {
                return true;
            }
            return false;
        }
    }
}
