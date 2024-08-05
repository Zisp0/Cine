using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CineAPI.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(int userId)
    {
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("La clave JWT no está configurada.");
        }

        var keyBytes = Encoding.ASCII.GetBytes(key);
        var tokenHandler = new JwtSecurityTokenHandler();

        int durationMinutes = int.Parse(_configuration["Jwt:DurationMinutes"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim("UserId", userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(durationMinutes),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}