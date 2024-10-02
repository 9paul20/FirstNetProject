using System.Security.Cryptography;
using System.Text;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseAPIController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterUserDto registerDto)
    {
        var validator = new RegisterUserDtoValidator(context);
        ValidationResult result = await validator.ValidateAsync(registerDto);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        byte[] passwordHash,
            passwordSalt;
        CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

        var user = new AppUser
        {
            UserName = registerDto.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok(
            new UserDTO
            {
                UserName = user.UserName,
                // Password = registerDto.Password,
                Token = tokenService.CreateToken(user),
                // TokenExpiration = DateTime.Now.AddDays(7),
            }
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] RegisterUserDto registerDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == registerDto.UserName);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        if (!VerifyPasswordHash(registerDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        return Ok(
            new UserDTO
            {
                UserName = user.UserName,
                // Password = registerDto.Password,
                Token = tokenService.CreateToken(user),
                // TokenExpiration = DateTime.Now.AddDays(7),
            }
        );
    }

    private void CreatePasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt
    )
    {
        using var hmac = new HMACSHA512();

        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        passwordSalt = hmac.Key;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
            {
                return false;
            }
        }

        return true;
    }
}
