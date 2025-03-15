using log4net;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;

namespace ShoppingBasket.Core.Services
{
    public class BasketService : IBasketService
    {
        private readonly Dictionary<string, decimal> _prices = new()
        {
            { "Soup", 0.65m },
            { "Bread", 0.80m },
            { "Milk", 1.30m },
            { "Apples", 1.00m }
        };

        private readonly IDiscountService _discountService;
        private readonly ILog _log;

        public BasketService(IDiscountService discountService, ILog log)
        {
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public decimal CalculateSubtotal(List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                _log.Error("Basket items cannot be null or empty.");
                throw new ArgumentException("Basket items cannot be null or empty.");
            }

            decimal subtotal = 0m;
            foreach (var item in basketItems)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    _log.Error("Item name cannot be null or empty.");
                    throw new ArgumentException("Item name cannot be null or empty.");
                }

                if (_prices.ContainsKey(item.Name))
                {
                    subtotal += _prices[item.Name] * item.Quantity;
                }
                else
                {
                    _log.Error($"Price for item '{item.Name}' not found.");
                    throw new KeyNotFoundException($"Price for item '{item.Name}' not found.");
                }
            }

            _log.Info($"Subtotal calculated: {subtotal}");
            return subtotal;
        }

        public decimal CalculateTotal(List<BasketItem> basketItems)
        {
            try
            {
                decimal subtotal = CalculateSubtotal(basketItems);
                decimal totalDiscount = _discountService.CalculateDiscounts(basketItems).Sum(d => d.DiscountAmount);
                decimal total = subtotal - totalDiscount;

                _log.Info($"Total calculated: {total}");
                return total;
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while calculating the total.", ex);
                throw new ApplicationException("An error occurred while calculating the total.", ex);
            }
        }

        public string GenerateReceipt(List<BasketItem> basketItems)
        {
            try
            {
                if (basketItems == null || basketItems.Count == 0)
                {
                    _log.Error("Basket items cannot be null or empty.");
                    throw new ArgumentException("Basket items cannot be null or empty.");
                }

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
                _log.Info("Receipt generated successfully.");
                return receipt;
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while generating the receipt.", ex);
                return $"Error generating receipt: {ex.Message}";
            }
        }
    }
}