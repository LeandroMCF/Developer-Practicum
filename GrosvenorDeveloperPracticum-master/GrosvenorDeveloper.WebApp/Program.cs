using Application;
using Application.Enums;
using Application.Models;
using Application.Services;
using GrosvenorDeveloper.WebApp.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddScoped<IDishManager, DishManager>();
builder.Services.AddScoped<IServer, Server>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedData(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var dishes = new List<Dish>
        {
            new Dish { DishName = "steak", Count = 1 },
            new Dish { DishName = "potato", Count = 2 },
            new Dish { DishName = "wine", Count = 1 },
            new Dish { DishName = "cake", Count = 1 },
            new Dish { DishName = "egg", Count = 1 },
            new Dish { DishName = "toast", Count = 2 },
            new Dish { DishName = "coffee", Count = 1 }
        };

        db.Dishes.AddRange(dishes);

        db.MenuItems.AddRange(
            new MenuItem { DishId = 1, Period = Period.evening }, 
            new MenuItem { DishId = 2, Period = Period.evening }, 
            new MenuItem { DishId = 3, Period = Period.evening }, 
            new MenuItem { DishId = 4, Period = Period.evening }, 
            new MenuItem { DishId = 5, Period = Period.morning }, 
            new MenuItem { DishId = 6, Period = Period.morning }, 
            new MenuItem { DishId = 7, Period = Period.morning } 
        );

        db.SaveChanges();
    }
}
