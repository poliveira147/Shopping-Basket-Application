
using ShoppingBasket.Core.Models;

namespace ShoppingBasket.Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<IEnumerable<BasketItem>> GetAllAsync();
        Task<BasketItem?> GetByIdAsync(int id);
        Task AddAsync(BasketItem item);
    }
}
