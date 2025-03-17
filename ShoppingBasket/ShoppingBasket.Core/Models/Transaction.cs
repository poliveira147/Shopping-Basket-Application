
namespace ShoppingBasket.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; } // Primary key
        public DateTime TransactionDate { get; set; } // Date and time of the transaction
        public decimal TotalAmount { get; set; } // Total amount of the transaction
        public List<BasketItem> Items { get; set; } = new List<BasketItem>(); // Items in the transaction
        public List<Discount> Discounts { get; set; } = new List<Discount>(); // Discounts applied
    }
}
