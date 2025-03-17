using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.DTOs
{
    public class DiscountDTO
    {
        public string ItemName { get; set; } // Name of the item the discount applies to
        public string Description { get; set; } // Description of the discount
        public decimal DiscountAmount { get; set; } // The amount deducted from the total price
    }
}
