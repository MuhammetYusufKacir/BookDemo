using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace BookDemo.Core.Models
{
    public class ImageUploadDTO
    {
        public int Id { get; set; }
        public IFormFile? image { get; set; }

    }
}
