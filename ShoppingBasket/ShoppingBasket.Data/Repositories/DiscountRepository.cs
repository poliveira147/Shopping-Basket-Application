
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using ShoppingBasket.Data.Database;

namespace ShoppingBasket.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ShoppingBasketDbContext _context;

        public DiscountRepository(ShoppingBasketDbContext context)
        {
            _context = context;
        }

        public async Task<Discount> GetByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task AddAsync(Discount entity)
        {
            await _context.Discounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}