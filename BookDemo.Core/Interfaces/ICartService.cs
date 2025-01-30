using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDTO>> GetCartAsync(string userId);
        Task<ApiResponse<CartDTO>> AddToCartAsync(string userId, int bookId, int quantity);
        Task<ApiResponse<CartDTO>> RemoveFromCartAsync(string userId, int bookId, int quantityToRemove);
        Task<ApiResponse<bool>> UpdateSoldStatusAsync(int cartId);
        Task<Cart> GetCartByIdAsync(int id);
        Task<CartSalesResponse> UpdateCartAsync(Cart cart);
    }
}
