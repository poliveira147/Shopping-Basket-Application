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

        private readonly IDiscountService _discountService;

        public BasketService(IDiscountService discountService)
        {
            _discountService = discountService;
        }

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

        public decimal CalculateTotal(List<BasketItem> basketItems)
        {
            decimal subtotal = CalculateSubtotal(basketItems);
            decimal totalDiscount = _discountService.CalculateDiscounts(basketItems).Sum(d => d.DiscountAmount);
            return subtotal - totalDiscount;
        }

        public string GenerateReceipt(List<BasketItem> basketItems)
        {
            decimal subtotal = CalculateSubtotal(basketItems);
            List<Discount> discounts = _discountService.CalculateDiscounts(basketItems);
            decimal total = subtotal - discounts.Sum(d => d.DiscountAmount);

            string receipt = "Receipt:\n";
            foreach (var item in basketItems)
            {
                receipt += $"{item.Quantity} x {item.Name} @ €{_prices[item.Name]:F2} each\n";
            }

            receipt += $"\nSubtotal: €{subtotal:F2}";

            if (discounts.Count > 0)
            {
                receipt += "\nDiscounts Applied:";
                foreach (var discount in discounts)
                {
                    receipt += $"\n- {discount.Description}: -€{discount.DiscountAmount:F2}";
                }
            }
            else
            {
                receipt += "\nNo Discounts Applied";
            }

            receipt += $"\nTotal: €{total:F2}";
            return receipt;
        }
    }
}
