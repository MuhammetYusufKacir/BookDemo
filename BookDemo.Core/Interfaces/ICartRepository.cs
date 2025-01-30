using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<Cart> GetCartAsync(string userId);
        Task AddToCartAsync(Cart cart);
        Task AddCartItemAsync(CartItem cartItem);
        Task<CartItem> GetCartItemAsync(int cartId, int bookId);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task UpdateCartAsync(Cart cart);
        Task<Cart> GetCartByIdAsync(int cartId);
        Task SaveChangesAsync();

    }
}
