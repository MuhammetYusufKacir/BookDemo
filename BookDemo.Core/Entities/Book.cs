using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BookDemo.Core.Models;

namespace BookDemo.Core.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public  Category Category { get; set; }      
        public string? ImagePath {  get; set; }
        public EntityStatus Status { get; set; }
              
    }
}
