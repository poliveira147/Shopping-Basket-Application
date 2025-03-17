using ShoppingBasket.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.DTOs
{
    public class TransactionDTO
    {
        public DateTime TransactionDate { get; set; } // Date and time of the transaction
        public decimal TotalAmount { get; set; } // Total amount of the transaction
        public List<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>(); // Items in the transaction
        public List<DiscountDTO> Discounts { get; set; } = new List<DiscountDTO>(); // Discounts applied
    }
}
