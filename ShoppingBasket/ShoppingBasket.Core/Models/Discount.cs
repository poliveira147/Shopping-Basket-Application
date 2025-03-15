using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.Models
{
    public class Discount
    {
        // Item the discount applies to
        public string ItemName { get; set; } = string.Empty;
        // Discount details
        public string Description { get; set; } = string.Empty;
        // Discounted value
        public decimal DiscountAmount { get; set; } 
    }
}
