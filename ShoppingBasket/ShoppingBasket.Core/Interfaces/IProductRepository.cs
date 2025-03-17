
using ShoppingBasket.Core.Models;

namespace ShoppingBasket.Core.Interfaces
{
    public interface IProductRepository
    {

        public Task<Product> GetByIdAsync(int id);

        public Task<Product> GetByNameAsync(string name);


        public Task<IEnumerable<Product>> GetAllAsync();


        public Task AddAsync(Product entity);

    }
}

