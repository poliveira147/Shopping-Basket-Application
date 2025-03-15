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

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("calculate-total")]
        public IActionResult CalculateTotal([FromBody] List<BasketItem> basketItems)
        {
            if (basketItems == null || basketItems.Count == 0)
            {
                return BadRequest("Basket cannot be empty.");
            }

            decimal total = _basketService.CalculateTotal(basketItems);
            return Ok(new { Total = total });
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
