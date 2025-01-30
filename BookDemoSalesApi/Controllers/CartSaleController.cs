using BookDemo.Core.Entities;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDemoSalesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartSaleController : Controller
    {
        private readonly ICartService _cartService;

        public CartSaleController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("update-sold")]
        public async Task<ApiResponse<Cart>> UpdateSoldStatusAsync([FromBody] Cart existingCart)
        {
            try
            {
                if (existingCart == null)
                {
                    return new ApiResponse<Cart>(false, null, "Cart not found.", 404);
                }

                existingCart.Sold = true;
                await _cartService.UpdateCartAsync(existingCart);

                return new ApiResponse<Cart>(true, existingCart, "Cart updated successfully.", 200);
            }
            catch (Exception ex)
            {
                return new ApiResponse<Cart>(false, null, ex.Message, 500);
            }
        }

    }
}
