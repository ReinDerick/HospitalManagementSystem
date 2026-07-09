using System.Security.Claims;
using System.Text;
using HospitalManagementSystem.Models.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagementSystem.Api.Users.Infrastructure;

public class TokenProvider(IConfiguration configuration)
{
    public string Create(HMUser hMUser)
    {
        string secretKey = configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, hMUser.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, hMUser.Email),
                new Claim("email-verified", hMUser.Email.ToString())
            ]),
            // FIXED: Changed "Jwt:TokenExpirationInMinutes" to "Jwt:ExpirationInMinutes"
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}