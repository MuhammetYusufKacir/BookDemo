using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDemo.Core.Models
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
