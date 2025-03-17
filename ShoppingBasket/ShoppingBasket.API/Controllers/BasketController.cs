using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Core.DTOs;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingBasket.API.Controllers
{
    [ApiController]
    [Route("api/basket")]
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
        public async Task<IActionResult> CalculateTotal([FromBody] List<BasketItemInputDTO> basketItems)
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

                // Convert BasketItemRequestDTO to BasketItem
                var basketItemList = basketItems.Select(item => new BasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList();

                // Fetch product details and calculate totals
                var subtotal = await _basketService.CalculateSubtotalAsync(basketItemList);
                var discounts = await _discountService.CalculateDiscountsAsync(basketItemList);
                var total = subtotal - discounts.Sum(d => d.DiscountAmount);

                // Map discounts to DiscountDTO
                var discountDTOs = discounts.Select(d => new DiscountDTO
                {
                    ItemName = d.ItemName,
                    Description = d.Description,
                    DiscountAmount = d.DiscountAmount
                }).ToList();

                return Ok(new
                {
                    Subtotal = subtotal,
                    Discounts = discountDTOs,
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
        public async Task<IActionResult> GenerateReceipt([FromBody] List<BasketItemInputDTO> basketItems)
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

                // Convert BasketItemRequestDTO to BasketItem
                var basketItemList = basketItems.Select(item => new BasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList();

                // Generate the receipt
                string receipt = await _basketService.GenerateReceiptAsync(basketItemList);
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