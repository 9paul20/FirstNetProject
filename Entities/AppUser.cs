using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Entities;

public class AppUser
{
    public int Id { get; set; }

    public required string UserName { get; set; }
}

public class AppUserValidator : AbstractValidator<AppUser>
{
    private readonly DataContext _context;

    public AppUserValidator(DataContext context)
    {
        _context = context;

        RuleFor(user => user.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .Length(3, 100).WithMessage("UserName must be between 3 and 100 characters.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("UserName must contain only letters and numbers.")
            .MustAsync(BeUniqueUserName).WithMessage("UserName must be unique.");
    }

    private async Task<bool> BeUniqueUserName(AppUser user, string userName, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(u => u.UserName == userName && u.Id != user.Id, cancellationToken);
    }
}

public class DeleteUserValidator : AbstractValidator<int>
{
    private readonly DataContext _context;

    public DeleteUserValidator(DataContext context)
    {
        _context = context;

        RuleFor(id => id)
            .MustAsync(async (id, cancellation) => await _context.Users.FindAsync(id) != null)
            .WithMessage("User with Id {PropertyValue} not found.");
    }
}