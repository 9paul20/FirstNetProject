using System;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(DataContext context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();

        if (users == null || users.Count == 0)
        {
            return NotFound("No users found.");
        }

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { message = $"User with Id {id} not found.", id = $"ESE {id}" });
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<AppUser>> CreateUser([FromBody] AppUser user)
    {
        var validator = new AppUserValidator(context);
        ValidationResult result = await validator.ValidateAsync(user);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AppUser>> UpdateUser(int id, AppUser user)
    {
        if (id != user.Id)
        {
            return BadRequest(new { message = "The provided Id does not match the user's Id." });
        }

        var validator = new AppUserValidator(context);
        ValidationResult validationResult = await validator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        context.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound(new { message = $"User with Id {id} not found." });
            }
        }

        return Ok(user);
    }

    private bool UserExists(int id)
    {
        return context.Users.Any(e => e.Id == id);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<AppUser>> DeleteUser(int id)
    {
        var validator = new DeleteUserValidator(context);
        var validationResult = await validator.ValidateAsync(id);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult.Errors });
        }

        var user = await context.Users.FindAsync(id);

        if (user != null)
        {
            context.Users.Remove(user);

            await context.SaveChangesAsync();

        }

        return Ok(user);
    }
}
