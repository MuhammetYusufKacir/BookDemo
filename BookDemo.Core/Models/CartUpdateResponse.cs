using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDemo.Core.Models
{
    public class CartUpdateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool Sold { get; set; }
    }
}
