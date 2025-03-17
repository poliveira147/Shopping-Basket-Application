using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using ShoppingBasket.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingBasket.Tests.Business
{
    [TestClass]
    public class DiscountServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IDiscountRepository> _discountRepositoryMock;
        private DiscountService _discountService;
        private Mock<ILog> _loggerMock;
        private ILog _logger;

        [TestInitialize]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _discountRepositoryMock = new Mock<IDiscountRepository>();

            // Initialize and assign the logger mock
            _loggerMock = new Mock<ILog>();
            _logger = _loggerMock.Object;

            _discountService = new DiscountService(
                _productRepositoryMock.Object,
                _discountRepositoryMock.Object,
                _logger
            );
        }

        /// <summary>
        /// Tests that discounts are correctly applied when a valid discount is available.
        /// </summary>
        [TestMethod]
        public async Task CalculateDiscounts_ValidDiscount_AppliesCorrectly()
        {
            var basketItems = new List<BasketItem>
            {
                new() { ProductId = 1, Quantity = 5 }
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _discountRepositoryMock.Setup(repo => repo.GetByDescriptionAsync("10% off apples"))
                .ReturnsAsync((Discount)null);

            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);

            Assert.AreEqual(1, discounts.Count);
            Assert.AreEqual("10% off apples", discounts[0].Description);
            Assert.AreEqual(0.50m, discounts[0].DiscountAmount);
        }

        /// <summary>
        /// Tests that an empty basket throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CalculateDiscounts_EmptyBasket_ThrowsException()
        {
            await _discountService.CalculateDiscountsAsync(new List<BasketItem>());
        }

        /// <summary>
        /// Tests that if a product in the basket does not exist in the repository, a KeyNotFoundException is thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task CalculateDiscounts_ProductNotFound_ThrowsException()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 99, Quantity = 1 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Product)null);

            await _discountService.CalculateDiscountsAsync(basketItems);
        }

        /// <summary>
        /// Tests that no discount is applied when the item is not eligible for any existing discounts.
        /// </summary>
        [TestMethod]
        public async Task CalculateDiscounts_NoEligibleDiscounts_ReturnsEmptyList()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 2, Quantity = 3 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Product { Id = 2, Name = "Bananas", Price = 1.50m });

            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);

            Assert.AreEqual(0, discounts.Count);
        }

        /// <summary>
        /// Tests that a multi-buy discount (Buy 2 soups, get 1 bread at half price) is applied correctly.
        /// </summary>
        [TestMethod]
        public async Task CalculateDiscounts_MultiBuyDiscount_AppliesCorrectly()
        {
            var basketItems = new List<BasketItem>
            {
                new() { ProductId = 3, Quantity = 2 }, // Soup
                new() { ProductId = 4, Quantity = 1 }  // Bread
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(new Product { Id = 3, Name = "Soup", Price = 1.00m });

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(4))
                .ReturnsAsync(new Product { Id = 4, Name = "Bread", Price = 2.00m });

            _discountRepositoryMock.Setup(repo => repo.GetByDescriptionAsync("Buy 2 soups, get 1 bread half price"))
                .ReturnsAsync((Discount)null);

            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);

            Assert.AreEqual(1, discounts.Count);
            Assert.AreEqual("Buy 2 soups, get 1 bread half price", discounts[0].Description);
            Assert.AreEqual(1.00m, discounts[0].DiscountAmount);
        }

        /// <summary>
        /// Tests that multiple discounts (e.g., apple discount and soup-bread discount) are correctly applied.
        /// </summary>
        [TestMethod]
        public async Task CalculateDiscounts_MultipleDiscounts_AppliesAll()
        {
            var basketItems = new List<BasketItem>
            {
                new() { ProductId = 1, Quantity = 5 }, // Apples
                new() { ProductId = 3, Quantity = 2 }, // Soup
                new() { ProductId = 4, Quantity = 1 }  // Bread
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(new Product { Id = 3, Name = "Soup", Price = 1.00m });

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(4))
                .ReturnsAsync(new Product { Id = 4, Name = "Bread", Price = 2.00m });

            _discountRepositoryMock.Setup(repo => repo.GetByDescriptionAsync("10% off apples"))
                .ReturnsAsync((Discount)null);

            _discountRepositoryMock.Setup(repo => repo.GetByDescriptionAsync("Buy 2 soups, get 1 bread half price"))
                .ReturnsAsync((Discount)null);

            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);

            Assert.AreEqual(2, discounts.Count);
            Assert.IsTrue(discounts.Any(d => d.Description == "10% off apples" && d.DiscountAmount == 0.50m));
            Assert.IsTrue(discounts.Any(d => d.Description == "Buy 2 soups, get 1 bread half price" && d.DiscountAmount == 1.00m));
        }

        /// <summary>
        /// Tests that a large quantity of discounted items still applies the correct discount.
        /// </summary>
        [TestMethod]
        public async Task CalculateDiscounts_LargeQuantity_AppliesCorrectly()
        {
            var basketItems = new List<BasketItem>
            {
                new() { ProductId = 1, Quantity = 1000 } // Apples
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _discountRepositoryMock.Setup(repo => repo.GetByDescriptionAsync("10% off apples"))
                .ReturnsAsync((Discount)null);

            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);

            Assert.AreEqual(1, discounts.Count);
            Assert.AreEqual("10% off apples", discounts[0].Description);
            Assert.AreEqual(100m, discounts[0].DiscountAmount);
        }

        /// <summary>
        /// Tests that a discount is not applied when the required conditions are not met (e.g., only one soup instead of two).
        /// </summary>
        [TestMethod]
        public async Task CalculateDiscounts_InsufficientConditions_NoDiscountApplied()
        {
            var basketItems = new List<BasketItem>
            {
                new() { ProductId = 3, Quantity = 1 }, // Only 1 soup
                new() { ProductId = 4, Quantity = 1 }  // Bread
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(new Product { Id = 3, Name = "Soup", Price = 1.00m });

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(4))
                .ReturnsAsync(new Product { Id = 4, Name = "Bread", Price = 2.00m });

            var discounts = await _discountService.CalculateDiscountsAsync(basketItems);

            Assert.AreEqual(0, discounts.Count);
        }
    }
}
