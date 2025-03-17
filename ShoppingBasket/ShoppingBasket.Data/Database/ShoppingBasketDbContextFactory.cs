using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ShoppingBasket.Data.Database
{
    public class ShoppingBasketDbContextFactory : IDesignTimeDbContextFactory<ShoppingBasketDbContext>
    {
        public ShoppingBasketDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingBasketDbContext>();

            // Point to the appsettings.json file in the API project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName) // Get the root project folder
                .AddJsonFile("ShoppingBasket.API/appsettings.json") // Correct path to appsettings.json in the API project
                .Build();

            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new ShoppingBasketDbContext(optionsBuilder.Options);
        }
    }
}
