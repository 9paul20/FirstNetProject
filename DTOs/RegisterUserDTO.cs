using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.DTOs;

public class RegisterUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private readonly DataContext _context;

    public RegisterUserDtoValidator(DataContext context)
    {
        _context = context;

        RuleFor(user => user.UserName)
            .NotEmpty()
            .WithMessage("UserName is required.")
            .Length(3, 100)
            .WithMessage("UserName must be between 3 and 100 characters.")
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("UserName must contain only letters and numbers.")
            .MustAsync(BeUniqueUserName)
            .WithMessage("UserName must be unique.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(6, 100)
            .WithMessage("Password must be between 6 and 100 characters.")
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,100}$")
            .WithMessage(
                "Password must contain at least one lowercase letter, one uppercase letter, and one number."
            );
    }

    private async Task<bool> BeUniqueUserName(
        RegisterUserDto user,
        string userName,
        CancellationToken cancellationToken
    )
    {
        return !await _context.Users.AnyAsync(u => u.UserName == userName, cancellationToken);
    }
}
