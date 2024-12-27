using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CsApi.Models.Domain;
using CsApi.Utils.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CsApi.Utils.Implementations;

public class JwtUtils(IOptions<JwtSettings> jwt)  : IJwtUtils
{
    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("IsActive", user.IsActive.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt.Value.Issuer,
            audience: jwt.Value.Audience,
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}