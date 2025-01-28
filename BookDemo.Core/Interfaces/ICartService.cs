using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDTO>> GetCartAsync(string userId);
        Task<ApiResponse<CartDTO>> AddToCartAsync(string userId, int bookId, int quantity);
        Task<ApiResponse<CartDTO>> RemoveFromCartAsync(string userId, int bookId, int quantityToRemove);
    

    }
}
