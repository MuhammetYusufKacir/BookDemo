using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using BookDemo.Infrastructure.Repositories;

namespace BookDemo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IBookRepository bookRepository, ICacheService cacheService, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }
        public static class CacheKeyHelper
        {
            public static string GetCacheKeyForCart(string userId)
            {
                return $"CartItems_{userId}";
            }
        }
        public async Task<ApiResponse<CartDTO>> GetCartAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiResponse<CartDTO>(false, null, "Invalid user ID.", 400);
                }

                string cacheKey = CacheKeyHelper.GetCacheKeyForCart(userId);
                var cacheData = await _cacheService.GetAsync<ApiResponse<CartDTO>>(cacheKey);
                if (cacheData != null)
                {
                    return cacheData;
                }

                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return new ApiResponse<CartDTO>(false, null, "Cart not found.", 404);
                }

                var cartDto = _mapper.Map<CartDTO>(cart);
              
                cartDto.TotalPrice = cartDto.CartItems.Sum(item => item.Price * item.Quantity);
                var apiResponse = new ApiResponse<CartDTO>(true, cartDto, "Cart retrieved successfully", 200);
                await _cacheService.SetAsync(cacheKey, apiResponse, TimeSpan.FromMinutes(10));

                return apiResponse;
            }
            catch (Exception ex)
            {

                return new ApiResponse<CartDTO>(false, null, ex.Message, 500);
            }
        }


        public async Task<ApiResponse<CartDTO>> AddToCartAsync(string userId, int bookId, int quantity)
        {
            try
            {
                string cacheKey = CacheKeyHelper.GetCacheKeyForCart(userId);
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        CrateDate = DateTime.UtcNow,
                        CartItem = new List<CartItem>()
                    };
                    await _cartRepository.AddToCartAsync(cart);
                }

                var cartItem = cart.CartItem.FirstOrDefault(ci => ci.BookId == bookId);
                if (cartItem == null)
                {
                    var book = await _bookRepository.GetByIdAsync(bookId);
                    if (book == null)
                        return new ApiResponse<CartDTO>(false, null, "Book not found.", 404);

                    cartItem = new CartItem
                    {
                        BookId = bookId,
                        Quantity = quantity,
                        Price = book.Price
                    };
                    cart.CartItem.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity += quantity;
                }

                await _cartRepository.SaveChangesAsync();

                var cartDto = _mapper.Map<CartDTO>(cart);
                cartDto.TotalPrice = cartDto.CartItems.Sum(item => item.Price * item.Quantity);
                await _cacheService.SetAsync(cacheKey, cartDto, TimeSpan.FromMinutes(10));

                return new ApiResponse<CartDTO>(true, cartDto, "Book added to cart successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartDTO>(false, null, ex.Message, 500);
            }
        }


        public async Task<ApiResponse<CartDTO>> RemoveFromCartAsync(string userId, int bookId, int quantityToRemove)
        {
            try
            {
                string cacheKey = CacheKeyHelper.GetCacheKeyForCart(userId);

                // Cache'den sepeti al
                var cacheCart = await _cacheService.GetAsync<CartDTO>(cacheKey);
                if (cacheCart == null)
                {
                    // Eğer cache boşsa, veritabanından sepeti getir
                    var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                    if (cart == null)
                        return new ApiResponse<CartDTO>(false, null, "Cart not found.", 404);

                    cacheCart = _mapper.Map<CartDTO>(cart);
                }

                var cartItem = cacheCart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
                if (cartItem == null)
                    return new ApiResponse<CartDTO>(false, null, "Book not found in cart.", 404);

                // Quantity kontrolü
                if (cartItem.Quantity <= quantityToRemove)
                {
                    // Eğer miktar eşitse veya daha azsa, tüm ürünü çıkar
                    cacheCart.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= quantityToRemove;
                }

                // Cache'i güncelle
                await _cacheService.SetAsync(cacheKey, cacheCart, TimeSpan.FromMinutes(10));

                // Veritabanında da güncelleme yap
                var dbCartForUpdate = await _cartRepository.GetCartByUserIdAsync(userId);
                if (dbCartForUpdate != null)
                {
                    var dbCartItem = dbCartForUpdate.CartItem.FirstOrDefault(ci => ci.BookId == bookId);
                    if (dbCartItem != null)
                    {
                        if (dbCartItem.Quantity <= quantityToRemove)
                        {
                            dbCartForUpdate.CartItem.Remove(dbCartItem);
                        }
                        else
                        {
                            dbCartItem.Quantity -= quantityToRemove;
                        }
                        await _cartRepository.UpdateCartAsync(dbCartForUpdate);
                    }
                }

                // Güncellenmiş CartDTO'yu oluştur
                var updatedCartDto = _mapper.Map<CartDTO>(dbCartForUpdate);

                return new ApiResponse<CartDTO>(true, updatedCartDto, "Book removed from cart successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartDTO>(false, null, ex.Message, 500);
            }
        }

    }
}
