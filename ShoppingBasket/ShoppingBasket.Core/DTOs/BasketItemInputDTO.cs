using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Core.DTOs
{
    public class BasketItemInputDTO
    {
        [Required]
        public int ProductId { get; set; } // ID of the product

        [Required]
        public int Quantity { get; set; } // Quantity of the product
    }
}
