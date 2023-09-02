using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(DrinkDbContext context)
        {
            context.DrinkItems.Add(new DrinkEntity() { Calories = 250, Type = "Espresso", Name = "Caramel Macchiato", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 120, Type = "Tea", Name = "Ice Green Tea Latte", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 420, Type = "Frappuccino", Name = "Java Chip Frappuccino", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 90, Type = "Refresher", Name = "Strawberry Acai Refresher", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 420, Type = "Espresso", Name = "White Chocolate Mocha", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 120, Type = "Espresso", Name = "Cappuccino", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 240, Type = "Tea", Name = "Chai Tea Latte", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 450, Type = "Frappuccino", Name = "Mocha Cookie Crumble Frappuccino", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 60, Type = "Coffee", Name = "Iced Coffee", Created = DateTime.Now });
            context.DrinkItems.Add(new DrinkEntity() { Calories = 180, Type = "Tea", Name = "Matcha Green Tea Latte", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
