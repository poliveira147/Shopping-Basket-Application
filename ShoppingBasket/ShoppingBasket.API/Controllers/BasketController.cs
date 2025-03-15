using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

                // Validate basket items for empty names
                foreach (var item in basketItems)
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        return BadRequest(new
                        {
                            errorCode = "VALIDATION_ERROR",
                            message = "Item name cannot be null or empty."
                        });
                    }
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
        public IActionResult GenerateReceipt([FromBody] List<BasketItem> basketItems)
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

                // Validate basket items for empty names
                foreach (var item in basketItems)
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        return BadRequest(new
                        {
                            errorCode = "VALIDATION_ERROR",
                            message = "Item name cannot be null or empty."
                        });
                    }
                }

                string receipt = _basketService.GenerateReceipt(basketItems);
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