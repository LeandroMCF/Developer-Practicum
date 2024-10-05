using Application.Enums;
using System.Collections.Generic;

namespace Application
{
    public class Order
    {
        public Order()
        {
            Dishes = new List<int>();
        }
        public List<int> Dishes { get; set; }
        public Period Period { get; set; }
    }
}