using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;

namespace BookDemo.Core.Models
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public string? ImagePath { get; set; }

    }
}
