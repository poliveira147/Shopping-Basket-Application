// BasketApp.Core/Models/BasketItemRequest.cs
using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Core.DTOs
{
    public class BasketItemDTO
    {
        public string ProductName { get; set; } // Name of the product
        public int Quantity { get; set; } // Quantity of the product
        public decimal Price { get; set; } // Price of the product
    }
}