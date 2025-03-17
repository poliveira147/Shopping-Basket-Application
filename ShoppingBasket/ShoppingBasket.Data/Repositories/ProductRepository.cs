

using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using ShoppingBasket.Data.Database;

namespace ShoppingBasket.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingBasketDbContext _context;

        public ProductRepository(ShoppingBasketDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}