﻿namespace IdentityUserService.Models;

public class LoginInputModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
