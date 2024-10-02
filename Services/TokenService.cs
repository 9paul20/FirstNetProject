using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey =
            config["TokenKey"] ?? throw new Exception("TokenKey not found in appsettings.json");

        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new Exception("TokenKey not found in appsettings.json");
        }
        else if (tokenKey.Length < 64)
        {
            throw new Exception("TokenKey must be at least 16 characters long");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        // var claims = new List<Claim> { new(JwtRegisteredClaimNames.NameId, user.UserName) };
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.UserName) };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
