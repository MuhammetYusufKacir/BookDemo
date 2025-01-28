using System.Security.Claims;
using BookDemo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _cartService.GetCartAsync(userId);
            return Ok(result);
        }
        [Authorize(Roles ="admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int bookId, int quantity)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _cartService.AddToCartAsync(userId, bookId, quantity);
            return Ok(result);
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(int bookId,int quantityToRemove)
        {
            var userId = User.Claims.FirstOrDefault(c=>c.Type== ClaimTypes.NameIdentifier)?.Value;
            var result = await _cartService.RemoveFromCartAsync(userId,bookId,quantityToRemove);
            return Ok(result);
        }

        
    }
}
