using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Interfaces
{
    public interface IDiscountService
    {
        List<Discount> CalculateDiscounts(List<BasketItem> basketItems);
    }
}
