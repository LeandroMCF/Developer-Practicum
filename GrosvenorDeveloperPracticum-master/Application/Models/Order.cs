using Application.Enums;
using System.Collections.Generic;

namespace Application.Models
{
    public class Order
    {
        public Order()
        {
            Dishes = new List<int>();
        }

        public int Id { get; set; }
        public List<int> Dishes { get; set; }
        public Period Period { get; set; }
    }
}