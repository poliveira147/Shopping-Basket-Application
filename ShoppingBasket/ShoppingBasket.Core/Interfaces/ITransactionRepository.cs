using ShoppingBasket.Core.Models;

namespace ShoppingBasket.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
    }
}
