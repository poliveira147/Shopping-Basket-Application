using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Core.Models;

namespace ShoppingBasket.Data.Database
{
    public class ShoppingBasketDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ShoppingBasketDbContext(DbContextOptions<ShoppingBasketDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between BasketItem and Product
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Product) // BasketItem has one Product
                .WithMany() // Product can have many BasketItems
                .HasForeignKey(bi => bi.ProductId) // Foreign key is ProductId
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Product if it has associated BasketItems

            // Configure the relationship between BasketItem and Transaction
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Transaction) // BasketItem has one Transaction
                .WithMany(t => t.Items) // Transaction has many BasketItems
                .HasForeignKey(bi => bi.TransactionId) // Foreign key is TransactionId
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: Deleting a Transaction deletes its BasketItems

            // Configure the relationship between Discount and Transaction
            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Transaction) // Discount has one Transaction
                .WithMany(t => t.Discounts) // Transaction has many Discounts
                .HasForeignKey(d => d.TransactionId) // Foreign key is TransactionId
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: Deleting a Transaction deletes its Discounts

            // Seed initial data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Soup", Price = 0.65m },
                new Product { Id = 2, Name = "Bread", Price = 0.80m },
                new Product { Id = 3, Name = "Milk", Price = 1.30m },
                new Product { Id = 4, Name = "Apples", Price = 1.00m }
            );
        }
    }
}