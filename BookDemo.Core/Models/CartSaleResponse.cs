using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;

namespace BookDemo.Core.Models
{
    public class CartSalesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool Sold { get; set; }
    }

}
