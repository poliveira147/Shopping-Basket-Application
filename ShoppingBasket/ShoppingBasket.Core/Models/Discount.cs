// BasketApp.Core/Models/Discount.cs
namespace ShoppingBasket.Core.Models
{
    public class Discount
    {
        public int Id { get; set; } // Primary key
        public string ItemName { get; set; } // Name of the item the discount applies to
        public string Description { get; set; } // Description of the discount
        public decimal DiscountAmount { get; set; } // The amount deducted from the total price
        public int TransactionId { get; set; } // Foreign key
        public Transaction Transaction { get; set; } // Navigation property
    }
}