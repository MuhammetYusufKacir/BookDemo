using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using BookDemo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookDemo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User?> Register(string name, string email, string passwordHash, string role)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return null; 
            }

            var newUser = new User
            {
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                Role = role
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
        public async Task<bool> Login(string email, string passwordHash)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
