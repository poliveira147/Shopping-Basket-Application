// ShoppingBasket.Core/Services/DiscountService.cs
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;


namespace ShoppingBasket.Core.Services
{

    public class DiscountService : IDiscountService
    {
        private readonly IProductRepository _productRepository;

        public DiscountService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Discount>> CalculateDiscountsAsync(List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                throw new ArgumentException("Basket items cannot be null or empty.");
            }

            List<Discount> discounts = new();

            // Fetch product details for all items in the basket
            foreach (var item in basketItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
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
                    discounts.Add(new Discount
                    {
                        ItemName = "Apples",
                        Description = "10% off apples",
                        DiscountAmount = appleDiscount
                    });
                }
            }

            // Multi-buy: Buy 2 soups, get 1 bread at half price
            var soupItem = basketItems.FirstOrDefault(i => i.Product.Name == "Soup");
            var breadItem = basketItems.FirstOrDefault(i => i.Product.Name == "Bread");

            if (soupItem != null && breadItem != null)
            {
                int discountBreadCount = soupItem.Quantity / 2; // Every 2 soups = 1 bread at discount
                decimal breadDiscount = Math.Min(discountBreadCount, breadItem.Quantity) * (breadItem.Product.Price / 2);

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