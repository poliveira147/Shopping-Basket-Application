using log4net;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private readonly ILog _log;

        public DiscountService(ILog log)
        {
            _log = log;
        }

        public List<Discount> CalculateDiscounts(List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                _log.Error("Basket items cannot be null or empty.");
                throw new ArgumentException("Basket items cannot be null or empty.");
            }

            List<Discount> discounts = new();

            foreach (var item in basketItems)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    _log.Error("Item name cannot be null or empty.");
                    throw new ArgumentException("Item name cannot be null or empty.");
                }
            }

            // Apples Discount (10% off)
            var appleItem = basketItems.FirstOrDefault(i => i.Name == "Apples");
            if (appleItem != null)
            {
                if (!_prices.ContainsKey("Apples"))
                {
                    _log.Error("Price for 'Apples' not found.");
                    throw new KeyNotFoundException("Price for 'Apples' not found.");
                }

                decimal appleDiscount = (_prices["Apples"] * appleItem.Quantity) * 0.10m;
                if (appleDiscount > 0)
                {
                    discounts.Add(new Discount
                    {
                        ItemName = "Apples",
                        Description = "10% off apples",
                        DiscountAmount = appleDiscount
                    });
                    _log.Info($"Applied 10% discount on apples: {appleDiscount}");
                }
            }

            // Multi-buy: Buy 2 soups, get 1 bread at half price
            var soupItem = basketItems.FirstOrDefault(i => i.Name == "Soup");
            var breadItem = basketItems.FirstOrDefault(i => i.Name == "Bread");

            if (soupItem != null && breadItem != null)
            {
                if (!_prices.ContainsKey("Soup") || !_prices.ContainsKey("Bread"))
                {
                    _log.Error("Price for 'Soup' or 'Bread' not found.");
                    throw new KeyNotFoundException("Price for 'Soup' or 'Bread' not found.");
                }

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
                    _log.Info($"Applied bread discount: {breadDiscount}");
                }
            }

            return discounts;
        }
    }
}