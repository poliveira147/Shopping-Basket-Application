
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
        public async Task<IActionResult> CalculateTotal([FromBody] List<BasketItem> basketItems)
        {
            try
            {
                if (basketItems == null || basketItems.Count == 0)
                {
                    return BadRequest(new
                    {
                        errorCode = "VALIDATION_ERROR",
                        message = "Basket cannot be empty."
                    });
                }

                // Validate required fields (ProductId and Quantity)
                foreach (var item in basketItems)
                {
                    if (item.ProductId <= 0)
                    {
                        return BadRequest(new
                        {
                            errorCode = "VALIDATION_ERROR",
                            message = "ProductId must be greater than 0."
                        });
                    }

                    if (item.Quantity <= 0)
                    {
                        return BadRequest(new
                        {
                            errorCode = "VALIDATION_ERROR",
                            message = "Quantity must be greater than 0."
                        });
                    }
                }

                // Fetch product details and calculate totals
                var subtotal = await _basketService.CalculateSubtotalAsync(basketItems);
                var discounts = await _discountService.CalculateDiscountsAsync(basketItems);
                var total = subtotal - discounts.Sum(d => d.DiscountAmount);

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
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    errorCode = "VALIDATION_ERROR",
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    errorCode = "ITEM_NOT_FOUND",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    errorCode = "INTERNAL_ERROR",
                    message = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }

        [HttpPost("generate-receipt")]
        public async Task<IActionResult> GenerateReceipt([FromBody] List<BasketItem> basketItems)
        {
            try
            {
                if (basketItems == null || basketItems.Count == 0)
                {
                    return BadRequest(new
                    {
                        errorCode = "VALIDATION_ERROR",
                        message = "Basket cannot be empty."
                    });
                }

                // Validate required fields (ProductId and Quantity)
                foreach (var item in basketItems)
                {
                    if (item.ProductId <= 0)
                    {
                        return BadRequest(new
                        {
                            errorCode = "VALIDATION_ERROR",
                            message = "ProductId must be greater than 0."
                        });
                    }

                    if (item.Quantity <= 0)
                    {
                        return BadRequest(new
                        {
                            errorCode = "VALIDATION_ERROR",
                            message = "Quantity must be greater than 0."
                        });
                    }
                }

                // Generate the receipt
                string receipt = await _basketService.GenerateReceiptAsync(basketItems);
                return Ok(new { Receipt = receipt });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    errorCode = "VALIDATION_ERROR",
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    errorCode = "ITEM_NOT_FOUND",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    errorCode = "INTERNAL_ERROR",
                    message = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}