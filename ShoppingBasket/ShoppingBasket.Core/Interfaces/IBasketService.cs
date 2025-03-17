
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
        Task<decimal> CalculateSubtotalAsync(List<BasketItem> basketItems);
        Task<decimal> CalculateTotalAsync(List<BasketItem> basketItems);
        Task<string> GenerateReceiptAsync(List<BasketItem> basketItems);
    }
}
