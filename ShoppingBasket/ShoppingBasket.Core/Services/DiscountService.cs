using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace ShoppingBasket.Core.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILog _logger;

        public DiscountService(
            IProductRepository productRepository,
            IDiscountRepository discountRepository,
            ILog logger)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task<List<Discount>> CalculateDiscountsAsync(List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                _logger.Warn("Attempted to calculate discounts for an empty or null basket.");
                throw new ArgumentException("Basket items cannot be null or empty.");
            }

            List<Discount> discounts = new();

            foreach (var item in basketItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    _logger.Error($"Product with ID '{item.ProductId}' not found.");
                    throw new KeyNotFoundException($"Product with ID '{item.ProductId}' not found.");
                }

                // Populate the Product field
                item.Product = product;
            }

            // Apples Discount (10% off)
            var appleItem = basketItems.FirstOrDefault(i => i.Product.Name == "Apples");
            if (appleItem != null)
            {
                decimal appleDiscount = (appleItem.Product.Price * appleItem.Quantity) * 0.10m;
                if (appleDiscount > 0)
                {
                    var existingAppleDiscount = await _discountRepository.GetByDescriptionAsync("10% off apples");
                    if (existingAppleDiscount == null)
                    {
                        var newDiscount = new Discount
                        {
                            ItemName = "Apples",
                            Description = "10% off apples",
                            DiscountAmount = appleDiscount
                        };
                        discounts.Add(newDiscount);
                    }
                    else
                    {
                        discounts.Add(existingAppleDiscount);
                    }
                    _logger.Info($"Applied discount for Apples: {appleDiscount:0.00}€");
                }
            }

            // Multi-buy: Buy 2 soups, get 1 bread at half price
            var soupItem = basketItems.FirstOrDefault(i => i.Product.Name == "Soup");
            var breadItem = basketItems.FirstOrDefault(i => i.Product.Name == "Bread");

            if (soupItem != null && breadItem != null)
            {
                int discountBreadCount = soupItem.Quantity / 2;
                decimal breadDiscount = Math.Min(discountBreadCount, breadItem.Quantity) * (breadItem.Product.Price / 2);

                if (breadDiscount > 0)
                {
                    var existingBreadDiscount = await _discountRepository.GetByDescriptionAsync("Buy 2 soups, get 1 bread half price");
                    if (existingBreadDiscount == null)
                    {
                        var newDiscount = new Discount
                        {
                            ItemName = "Bread",
                            Description = "Buy 2 soups, get 1 bread half price",
                            DiscountAmount = breadDiscount
                        };
                        discounts.Add(newDiscount);
                    }
                    else
                    {
                        discounts.Add(existingBreadDiscount);
                    }
                    _logger.Info($"Applied discount for Bread: {breadDiscount:0.00}€");
                }
            }

            _logger.Info($"Total discounts applied: {discounts.Sum(d => d.DiscountAmount):0.00}€");
            return discounts;
        }
    }
}
