using BookDemo.Core.Models;

namespace BookDemo.Core.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserDTO>> RegisterAsync(string name, string email, string password, string role);
        Task<ApiResponse<string>> LoginAsync(string email, string password);
        Task<ApiResponse<UserDTO?>> GetUserByEmailAsync(string email);
        Task<ApiResponse<string>> GenerateJwtToken(User user);
    }
}
