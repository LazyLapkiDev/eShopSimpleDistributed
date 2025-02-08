namespace IdentityUserService.Models;

public class RegisterInputModel
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
