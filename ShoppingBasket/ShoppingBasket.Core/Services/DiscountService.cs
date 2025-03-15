using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly Dictionary<string, decimal> _prices = new()
        {
            { "Soup", 0.65m },
            { "Bread", 0.80m },
            { "Milk", 1.30m },
            { "Apples", 1.00m }
        };

        public List<Discount> CalculateDiscounts(List<BasketItem> basketItems)
        {
            List<Discount> discounts = new();

            // Apples Discount (10% off)
            var appleItem = basketItems.FirstOrDefault(i => i.Name == "Apples");
            if (appleItem != null)
            {
                decimal appleDiscount = (_prices["Apples"] * appleItem.Quantity) * 0.10m;
                if (appleDiscount > 0)
                {
                    discounts.Add(new Discount
                    {
                        ItemName = "Apples",
                        Description = "10% off apples",
                        DiscountAmount = appleDiscount
                    });
                }
            }

            // Multi-buy: Buy 2 soups, get 1 bread at half price
            var soupItem = basketItems.FirstOrDefault(i => i.Name == "Soup");
            var breadItem = basketItems.FirstOrDefault(i => i.Name == "Bread");

            if (soupItem != null && breadItem != null)
            {
                int discountBreadCount = soupItem.Quantity / 2; // Every 2 soups = 1 bread at discount
                decimal breadDiscount = Math.Min(discountBreadCount, breadItem.Quantity) * (_prices["Bread"] / 2);

                if (breadDiscount > 0)
                {
                    discounts.Add(new Discount
                    {
                        ItemName = "Bread",
                        Description = "Buy 2 soups, get 1 bread half price",
                        DiscountAmount = breadDiscount
                    });
                }
            }

            return discounts;
        }
    }
}
