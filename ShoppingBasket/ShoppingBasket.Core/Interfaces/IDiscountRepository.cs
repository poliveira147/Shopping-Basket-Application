using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Interfaces
{
    public interface IDiscountRepository
    {
        public Task<Discount> GetByIdAsync(int id);


        public Task<IEnumerable<Discount>> GetAllAsync();


        public Task AddAsync(Discount entity);
    }
}
