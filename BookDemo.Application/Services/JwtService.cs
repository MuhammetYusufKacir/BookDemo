using BookDemo.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService
{
    private readonly JwtSettings _jwtSettings;
    public JwtService(IOptions<JwtSettings> jwtSettings, IConfiguration configuration)
    {
        _jwtSettings = jwtSettings.Value;

    }

    public string GenerateToken(User user)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
            throw new ArgumentNullException(nameof(_jwtSettings.Key), "JWT Key cannot be null or empty.");

        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");

        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);


        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
        new Claim(JwtRegisteredClaimNames.Name, user.Name), 
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
        new Claim(ClaimTypes.Role, user.Role) 
    };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

}
