﻿
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
        Task<List<Discount>> CalculateDiscountsAsync(List<BasketItem> basketItems);
    }
}
