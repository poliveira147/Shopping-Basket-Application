
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using ShoppingBasket.Data.Database;

namespace ShoppingBasket.Data.Repositories
{
    public class BasketRepository:IBasketRepository
    {
        private readonly ShoppingBasketDbContext _context;

        public BasketRepository(ShoppingBasketDbContext context)
        {
            _context = context;
        }

        public async Task<BasketItem> GetByIdAsync(int id)
        {
            return await _context.BasketItems.FindAsync(id);
        }

        public async Task<IEnumerable<BasketItem>> GetAllAsync()
        {
            return await _context.BasketItems.ToListAsync();
        }

        public async Task AddAsync(BasketItem entity)
        {
            await _context.BasketItems.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}