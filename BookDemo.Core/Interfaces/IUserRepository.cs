using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> Register(string name, string email, string password, string role);
        Task<bool> Login(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
    

    }
}
