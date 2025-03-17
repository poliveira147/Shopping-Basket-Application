// ShoppingBasket.Core/Services/BasketService.cs
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Services
{
    public class BasketService : IBasketService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountService _discountService; 
        private readonly ITransactionRepository _transactionRepository;

        public BasketService(IProductRepository productRepository, IDiscountService discountService, ITransactionRepository transactionRepository)
        {
            _productRepository = productRepository;
            _discountService = discountService;
            _transactionRepository = transactionRepository;
        }

        public async Task<decimal> CalculateSubtotalAsync(List<BasketItem> basketItems)
        {
            if (basketItems == null || !basketItems.Any())
            {
                throw new ArgumentException("Basket items cannot be null or empty.");
            }

            decimal subtotal = 0m;
            foreach (var item in basketItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID '{item.ProductId}' not found.");
                }

                // Populate the Product field
                item.Product = product;

                subtotal += product.Price * item.Quantity;
            }
            return subtotal;
        }

        public async Task<decimal> CalculateTotalAsync(List<BasketItem> basketItems)
        {
            decimal subtotal = await CalculateSubtotalAsync(basketItems);
            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);
            decimal totalDiscount = discounts.Sum(d => d.DiscountAmount);
            return subtotal - totalDiscount;
        }

        public async Task<string> GenerateReceiptAsync(List<BasketItem> basketItems)
        {
            var subtotal = await CalculateSubtotalAsync(basketItems);
            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);
            var total = await CalculateTotalAsync(basketItems);

            // Create a new Transaction
            var transaction = new Transaction
            {
                TransactionDate = DateTime.UtcNow, // Set the transaction date to the current time
                TotalAmount = total, // Set the total amount
                Items = basketItems, // Add the basket items to the transaction
                Discounts = discounts // Add the discounts to the transaction
            };

            // Save the transaction to the database
            await _transactionRepository.AddAsync(transaction);

            var receipt = $"Subtotal: €{subtotal:0.00}\n";
            if (discounts.Any())
            {
                receipt += "Discounts:\n";
                foreach (var discount in discounts)
                {
                    receipt += $"{discount.Description}: -€{discount.DiscountAmount:0.00}\n";
                }
            }
            receipt += $"Total: €{total:0.00}";

            return receipt;
        }
    }
}