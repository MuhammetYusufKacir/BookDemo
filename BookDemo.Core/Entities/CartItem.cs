using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDemo.Core.Entities
{
    public class CartItem : BaseEntity
    {
        
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public Book Book { get; set; }
        public Cart Cart { get; set; }
    }
}
