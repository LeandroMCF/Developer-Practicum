﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Inputs;
using Application.Models;

namespace Application
{

    public interface IDishManager
    {
        /// <summary>
        /// Constructs a list of dishes, each dish with a name and a count
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        List<Dish> GetDishes(Order order);
        string SeeMorninMenuMock();
        string SeeEveningMenuMock();
        Task<object> GetFullMenu();
        Task AddDishToMenu(AddDish dish);
    }
}