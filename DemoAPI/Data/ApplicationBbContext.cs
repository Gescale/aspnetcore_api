using DemoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //Empty for now
        }

        public DbSet<Shirt> Shirts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //data seeding
            modelBuilder.Entity<Shirt>().HasData(
                    new Shirt { ShirtId = 1, BrandName = "Country Road", Colour = "Red", Size = 34, Price = 199, Gender = "men" },
                    new Shirt { ShirtId = 2, BrandName = "Zara", Colour = "Blue", Size = 28, Price = 249, Gender = "women" },
                    new Shirt { ShirtId = 3, BrandName = "Woolworths", Colour = "Green", Size = 30, Price = 149, Gender = "men" },
                    new Shirt { ShirtId = 4, BrandName = "Polo", Colour = "Black", Size = 32, Price = 299, Gender = "women" },
                    new Shirt { ShirtId = 5, BrandName = "Levis", Colour = "White", Size = 36, Price = 176, Gender = "men" },
                    new Shirt { ShirtId = 6, BrandName = "Guess", Colour = "Yellow", Size = 26, Price = 219, Gender = "men" }
            );
        }
    }
}
