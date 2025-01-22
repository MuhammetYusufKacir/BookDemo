using BookDemo.Core.Models;
using BookDemo.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BookDemoAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        public AuthController(JwtService jwtService, AppDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("E-posta ve şifre gereklidir.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Kullanıcı bulunamadı.");
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Hatalı şifre.");
            }

            // Kullanıcı doğrulandı, JWT oluştur
            var token = _jwtService.GenerateToken(user);

            return Ok(new { Token = token });
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Geçersiz kayıt bilgileri.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return BadRequest("Bu email adresi zaten kullanımda.");
            }

  
            string passwordHash = HashPassword(request.Password);

            var newUser = new User
            {
               Name = request.Name,
               Email = request.Email,
                PasswordHash = passwordHash,
               Role = request.Role,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Kullanıcı başarıyla kaydedildi."});
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = Convert.ToBase64String(hashedBytes);
                return hashedPassword == storedHash;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            public string? Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }    
        }
    }
}
