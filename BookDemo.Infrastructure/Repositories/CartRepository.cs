using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using BookDemo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookDemo.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                 .Include(c => c.CartItem)
                 .ThenInclude(ci => ci.Book)
                 .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _context.Carts
           .Include(c => c.CartItem)
           .ThenInclude(ci => ci.BookId)
           .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddToCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }
        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }
        public async Task<CartItem> GetCartItemAsync(int cartId, int bookId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId== cartId && ci.BookId == bookId);
        }
        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
