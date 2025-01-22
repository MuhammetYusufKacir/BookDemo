using AutoMapper;
using BookDemo.Core.Interfaces;
using BookDemo.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _IHttpContextAccessor;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, IConfiguration configuration, IHttpContextAccessor IHttpContextAccessor)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _IHttpContextAccessor = IHttpContextAccessor;
    }

    public async Task<ApiResponse<string>> GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(secret))
{
    throw new ArgumentNullException(nameof(secret), "Jwt:Secret configuration is missing or empty.");
}
var key = Encoding.UTF8.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return new ApiResponse<string>(true, jwtToken, "Token generated successfully.", 200);
    }

    // Diğer IUserService metodlarını implement ettiniz
    public async Task<ApiResponse<UserDTO>> RegisterAsync(string name, string email, string password, string role)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            return new ApiResponse<UserDTO>(false, null, "Email already exists.", 400);
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        role = "user";
        var newUser = await _userRepository.Register(name, email, hashedPassword, role);

        if (newUser == null)
        {
            return new ApiResponse<UserDTO>(false, null, "User registration failed.", 500);
        }

        var userDto = _mapper.Map<UserDTO>(newUser);
        return new ApiResponse<UserDTO>(true, userDto, "User registered successfully.", 201);
    }

    public async Task<ApiResponse<string>> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return new ApiResponse<string>(false, null, "Invalid email or password.", 401);
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new ApiResponse<string>(false, null, "Invalid email or password.", 401);
        }

        var tokenResponse = await GenerateJwtToken(user); // GenerateJwtToken'ı çağırıyoruz
        return tokenResponse;
    }

    public async Task<ApiResponse<UserDTO>> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return new ApiResponse<UserDTO>(false, null, "User not found.", 404);
        }

        var userDto = _mapper.Map<UserDTO>(user);
        return new ApiResponse<UserDTO>(true, userDto, "User found.", 200);
    }

}
