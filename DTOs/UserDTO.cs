using System;

namespace WebApi.DTOs;

public class UserDTO
{
    public required string UserName { get; set; }

    // public required string Password { get; set; }
    public required string Token { get; set; }
    // public DateTime TokenExpiration { get; set; }
}
