using AutoMapper;
using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;


namespace BookDemo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly ExternalApiService _externalApiService;

        public CartService(ICartRepository cartRepository, IBookRepository bookRepository, ICacheService cacheService, IMapper mapper, ExternalApiService externalApiService)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _cacheService = cacheService;
            _mapper = mapper;
            _externalApiService = externalApiService;
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
                if (cart.Sold == true)
                    return new ApiResponse<CartDTO>(false, null, "Cart has been purchased", 404);

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
                if (cart.Sold == true)
                    return new ApiResponse<CartDTO>(false, null, "Cart has been purchased, you cannot add books.", 404);
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


                var cacheCart = await _cacheService.GetAsync<CartDTO>(cacheKey);
                if (cacheCart == null)
                {
                    var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                    if (cart == null)
                        return new ApiResponse<CartDTO>(false, null, "Cart not found.", 404);

                    cacheCart = _mapper.Map<CartDTO>(cart);
                }

                var cartItem = cacheCart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
                if (cartItem == null)
                    return new ApiResponse<CartDTO>(false, null, "Book not found in cart.", 404);


                if (cartItem.Quantity <= quantityToRemove)
                {
                    cacheCart.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= quantityToRemove;
                }


                await _cacheService.SetAsync(cacheKey, cacheCart, TimeSpan.FromMinutes(10));
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

                var updatedCartDto = _mapper.Map<CartDTO>(dbCartForUpdate);

                return new ApiResponse<CartDTO>(true, updatedCartDto, "Book removed from cart successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartDTO>(false, null, ex.Message, 500);
            }
        }


        public async Task<ApiResponse<bool>> UpdateSoldStatusAsync(int cartId)
        {
            try
            {
                var cart = await _cartRepository.GetCartByIdAsync(cartId);
                if (cart == null)
                {
                    return new ApiResponse<bool>(false, "Cart not found.");
                }

                if (cart.Sold == true)
                    return new ApiResponse<bool>(false, "Cart has been purchased.");
                var externalApiResponse = await _externalApiService.UpdateSoldStatusAsync(cart);

                if (externalApiResponse == null || !externalApiResponse.Success)
                {
                    return new ApiResponse<bool>(false, externalApiResponse?.Message ?? "Error updating sold status.");
                }

                cart.Sold = externalApiResponse.Sold;

                return new ApiResponse<bool>(true, "Sold status updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}");
            }
        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            var cart = await _cartRepository.GetCartByIdAsync(id);
            return cart;
        }

        public async Task<CartSalesResponse> UpdateCartAsync(Cart cart)
        {
            try
            {

                var existingCart = await _cartRepository.GetCartByIdAsync(cart.Id);
                if (existingCart == null)
                {
                    return new CartSalesResponse
                    {
                        Success = false,
                        Message = "Cart not found."
                    };
                }


                existingCart.Sold = true;
                await _cartRepository.UpdateCartAsync(existingCart);


                return new CartSalesResponse
                {
                    Success = true,
                    Message = "Sold status updated successfully.",
                    Sold = existingCart.Sold
                };
            }
            catch (Exception ex)
            {
                return new CartSalesResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}

