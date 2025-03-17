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
    public class BasketServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IDiscountService> _discountServiceMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private BasketService _basketService;
        private Mock<ILog> _loggerMock;
        private ILog _logger;

        [TestInitialize]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _discountServiceMock = new Mock<IDiscountService>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            // Initialize and assign the logger mock
            _loggerMock = new Mock<ILog>();
            _logger = _loggerMock.Object;

            _basketService = new BasketService(
                _productRepositoryMock.Object,
                _discountServiceMock.Object,
                _transactionRepositoryMock.Object,
                _logger 
            );
        }

        /// <summary>
        /// Tests that the subtotal is correctly calculated by summing the prices of all items in the basket.
        /// </summary>
        [TestMethod]
        public async Task CalculateSubtotal_CorrectlySumsItems()
        {
            var basketItems = new List<BasketItem>
            {
                new() { ProductId = 1, Quantity = 3 },
                new() { ProductId = 2, Quantity = 2 }
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Product { Id = 2, Name = "Soup", Price = 2.00m });

            var subtotal = await _basketService.CalculateSubtotalAsync(basketItems);

            Assert.AreEqual(7.00m, subtotal);
        }

        /// <summary>
        /// Tests that subtotal calculation handles an empty basket correctly by throwing an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CalculateSubtotal_EmptyBasket_ThrowsException()
        {
            await _basketService.CalculateSubtotalAsync(new List<BasketItem>());
        }

        /// <summary>
        /// Tests that an item not found in the repository throws a KeyNotFoundException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task CalculateSubtotal_UnrecognizedItem_ThrowsException()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 99, Quantity = 1 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Product)null); // Simulate missing product

            await _basketService.CalculateSubtotalAsync(basketItems);
        }

        /// <summary>
        /// Tests that discounts are correctly applied when calculating the total price.
        /// </summary>
        [TestMethod]
        public async Task CalculateTotal_AppliesDiscountsCorrectly()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 1, Quantity = 5 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _discountServiceMock.Setup(service => service.CalculateDiscountsAsync(basketItems))
                .ReturnsAsync(new List<Discount>
                {
                    new() { Description = "10% off apples", DiscountAmount = 0.50m }
                });

            var total = await _basketService.CalculateTotalAsync(basketItems);

            Assert.AreEqual(4.50m, total);
        }

        /// <summary>
        /// Tests that total price calculation correctly handles a basket with multiple discounts.
        /// </summary>
        [TestMethod]
        public async Task CalculateTotal_MultipleDiscounts_AppliesAllCorrectly()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 1, Quantity = 10 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _discountServiceMock.Setup(service => service.CalculateDiscountsAsync(basketItems))
                .ReturnsAsync(new List<Discount>
                {
                    new() { Description = "10% off apples", DiscountAmount = 1.00m },
                    new() { Description = "Extra seasonal discount", DiscountAmount = 2.00m }
                });

            var total = await _basketService.CalculateTotalAsync(basketItems);

            Assert.AreEqual(7.00m, total);
        }

        /// <summary>
        /// Tests that the receipt is generated in the correct format, including item details, subtotal, and total.
        /// </summary>
        [TestMethod]
        public async Task GenerateReceipt_ReturnsCorrectFormat()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 1, Quantity = 5 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Apples", Price = 1.00m });

            _discountServiceMock.Setup(service => service.CalculateDiscountsAsync(basketItems))
                .ReturnsAsync(new List<Discount> { new() { Description = "10% off apples", DiscountAmount = 0.50m } });

            _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var receipt = await _basketService.GenerateReceiptAsync(basketItems);

            StringAssert.Contains(receipt, "Subtotal: 5.00€");
            StringAssert.Contains(receipt, "10% off apples: -0.50€");
            StringAssert.Contains(receipt, "Total: 4.50€");
        }

        /// <summary>
        /// Tests that the receipt still prints properly when no discounts are applied.
        /// </summary>
        [TestMethod]
        public async Task GenerateReceipt_NoDiscounts_ReceiptFormatCorrect()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 1, Quantity = 5 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Bananas", Price = 1.00m });

            _discountServiceMock.Setup(service => service.CalculateDiscountsAsync(basketItems))
                .ReturnsAsync(new List<Discount>());

            _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var receipt = await _basketService.GenerateReceiptAsync(basketItems);

            StringAssert.Contains(receipt, "Subtotal: 5.00€");
            StringAssert.DoesNotMatch(receipt, new System.Text.RegularExpressions.Regex("Discounts:"));
            StringAssert.Contains(receipt, "Total: 5.00€");
        }

        /// <summary>
        /// Tests that the receipt handles large quantities of items properly without formatting issues.
        /// </summary>
        [TestMethod]
        public async Task GenerateReceipt_LargeQuantities_ReceiptDisplaysCorrectly()
        {
            var basketItems = new List<BasketItem> { new() { ProductId = 1, Quantity = 5000 } };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "Water Bottles", Price = 0.50m });

            _discountServiceMock.Setup(service => service.CalculateDiscountsAsync(basketItems))
                .ReturnsAsync(new List<Discount>());

            _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var receipt = await _basketService.GenerateReceiptAsync(basketItems);

            StringAssert.Contains(receipt, "Subtotal: 2500.00€");
            StringAssert.Contains(receipt, "Total: 2500.00€");
        }
    }
}
