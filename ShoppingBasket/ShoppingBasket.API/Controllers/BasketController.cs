using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;

namespace ShoppingBasket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IDiscountService _discountService;

        public BasketController(IBasketService basketService, IDiscountService discountService)
        {
            _basketService = basketService;
            _discountService = discountService;
        }

        [HttpPost("calculate-total")]
        public IActionResult CalculateTotal([FromBody] List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                return BadRequest("Basket cannot be empty.");
            }

            decimal subtotal = _basketService.CalculateSubtotal(basketItems);
            var discounts = _discountService.CalculateDiscounts(basketItems);
            decimal total = subtotal - discounts.Sum(d => d.DiscountAmount);

            return Ok(new
            {
                Subtotal = subtotal,
                Discounts = discounts.Select(d => new
                {
                    d.Description,
                    Amount = d.DiscountAmount
                }),
                Total = total
            });
        }

        [HttpPost("generate-receipt")]
        public IActionResult GenerateReceipt([FromBody] List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                return BadRequest("Basket cannot be empty.");
            }

            string receipt = _basketService.GenerateReceipt(basketItems);
            return Ok(new { Receipt = receipt });
        }
    }
}