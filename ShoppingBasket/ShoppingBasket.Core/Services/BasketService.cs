using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Services
{
    public class BasketService: IBasketService
    {
        private readonly Dictionary<string, decimal> _prices = new()
        {
            { "Soup", 0.65m },
            { "Bread", 0.80m },
            { "Milk", 1.30m },
            { "Apples", 1.00m }
        };

        public decimal CalculateSubtotal(List<BasketItem> basketItems)
        {
            decimal subtotal = 0m;

            foreach (var item in basketItems)
            {
                if (_prices.ContainsKey(item.Name))
                {
                    subtotal += _prices[item.Name] * item.Quantity;
                }
            }

            return subtotal;
        }

        public decimal CalculateDiscounts(List<BasketItem> basketItems)
        {
            decimal discount = 0m;

            foreach (var item in basketItems)
            {
                if (item.Name == "Apples")
                {
                    discount += (_prices["Apples"] * item.Quantity) * 0.10m; // 10% discount on apples
                }
            }

            // Multi-buy discount: Buy 2 soups, get bread at half price
            var soupItem = basketItems.FirstOrDefault(i => i.Name == "Soup");
            var breadItem = basketItems.FirstOrDefault(i => i.Name == "Bread");

            if (soupItem != null && breadItem != null)
            {
                int discountBreadCount = soupItem.Quantity / 2;
                discount += Math.Min(discountBreadCount, breadItem.Quantity) * (_prices["Bread"] / 2);
            }

            return discount;
        }

        public decimal CalculateTotal(List<BasketItem> basketItems)
        {
            decimal subtotal = CalculateSubtotal(basketItems);
            decimal discount = CalculateDiscounts(basketItems);
            return subtotal - discount;
        }

        public string GenerateReceipt(List<BasketItem> basketItems)
        {
            decimal subtotal = CalculateSubtotal(basketItems);
            decimal discount = CalculateDiscounts(basketItems);
            decimal total = subtotal - discount;

            string receipt = "Receipt:\n";
            foreach (var item in basketItems)
            {
                receipt += $"{item.Quantity} x {item.Name}\n";
            }
            receipt += $"\nSubtotal: €{subtotal:F2}";
            receipt += $"\nDiscounts: -€{discount:F2}";
            receipt += $"\nTotal: €{total:F2}";

            return receipt;
        }
    }
}
