using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace GrosvenorDeveloper.WebApp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.Dish)
                .WithMany()
                .HasForeignKey(mi => mi.DishId);
        }
    }
}
