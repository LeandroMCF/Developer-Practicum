namespace Application.Models
{
    /// <summary>
    /// Contains a dish by name and number of times the dish has been ordered
    /// </summary>
    public class Dish
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public int Count { get; set; }
    }
}