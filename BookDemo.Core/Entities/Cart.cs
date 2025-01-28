using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDemo.Core.Entities
{
    public class Cart : BaseEntity
    {   
        public string UserId { get; set; }
        public DateTime CrateDate { get; set; }

        public ICollection<CartItem> CartItem { get; set; }
    }
}
