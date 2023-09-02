using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class DrinkDbContext : DbContext
    {
        public DrinkDbContext(DbContextOptions<DrinkDbContext> options)
            : base(options)
        {
        }

        public DbSet<DrinkEntity> DrinkItems { get; set; } = null!;
    }
}
