using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace ShoppingBasket.Core.Services
{
    public class BasketService : IBasketService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountService _discountService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILog _logger;

        public BasketService(
            IProductRepository productRepository,
            IDiscountService discountService,
            ITransactionRepository transactionRepository,
            ILog logger)
        {
            _productRepository = productRepository;
            _discountService = discountService;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Calculates the subtotal of the basket by summing the price of each item multiplied by its quantity.
        /// Throws an exception if the basket is empty or null.
        /// </summary>
        /// <param name="basketItems">List of items in the basket.</param>
        /// <returns>The subtotal amount.</returns>
        public async Task<decimal> CalculateSubtotalAsync(List<BasketItem> basketItems)
        {
            if (basketItems == null || !basketItems.Any())
            {
                _logger.Warn("Attempted to calculate subtotal for an empty or null basket.");
                throw new ArgumentException("Basket items cannot be null or empty.");
            }

            decimal subtotal = 0m;
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
                subtotal += product.Price * item.Quantity;
            }

            _logger.Info($"Calculated subtotal: {subtotal:0.00}€");
            return subtotal;
        }

        /// <summary>
        /// Calculates the total amount of the basket by subtracting the total discounts from the subtotal.
        /// </summary>
        /// <param name="basketItems">List of items in the basket.</param>
        /// <returns>The total amount after applying discounts.</returns>
        public async Task<decimal> CalculateTotalAsync(List<BasketItem> basketItems)
        {
            decimal subtotal = await CalculateSubtotalAsync(basketItems);
            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);
            decimal totalDiscount = discounts.Sum(d => d.DiscountAmount);
            decimal total = subtotal - totalDiscount;

            _logger.Info($"Total after discounts: {total:0.00}€ (Discounts applied: {totalDiscount:0.00}€)");
            return total;
        }

        /// <summary>
        /// Generates a receipt for the basket, including subtotal, discounts, and total amount.
        /// Saves the transaction to the database.
        /// </summary>
        /// <param name="basketItems">List of items in the basket.</param>
        /// <returns>A formatted receipt string.</returns>
        public async Task<string> GenerateReceiptAsync(List<BasketItem> basketItems)
        {
            var subtotal = await CalculateSubtotalAsync(basketItems);
            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);
            var total = await CalculateTotalAsync(basketItems);

            // Create a new Transaction
            var transaction = new Transaction
            {
                TransactionDate = DateTime.UtcNow,
                TotalAmount = total,
                Items = basketItems,
                Discounts = discounts
            };

            // Save the transaction to the database
            await _transactionRepository.AddAsync(transaction);
            _logger.Info("Transaction successfully saved to the database.");

            var receipt = $"Subtotal: {subtotal:0.00}€\n";
            if (discounts.Any())
            {
                receipt += "Discounts:\n";
                foreach (var discount in discounts)
                {
                    receipt += $"{discount.Description}: -{discount.DiscountAmount:0.00}€\n";
                }
            }
            receipt += $"Total: {total:0.00}€";

            _logger.Info("Receipt generated successfully.");
            return receipt;
        }
    }
}