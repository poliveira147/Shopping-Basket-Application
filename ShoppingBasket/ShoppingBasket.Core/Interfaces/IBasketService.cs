using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Interfaces
{
    public interface IBasketService
    {
        decimal CalculateSubtotal(List<BasketItem> basketItems);
        decimal CalculateDiscounts(List<BasketItem> basketItems);
        decimal CalculateTotal(List<BasketItem> basketItems);
        string GenerateReceipt(List<BasketItem> basketItems);
    }
}
