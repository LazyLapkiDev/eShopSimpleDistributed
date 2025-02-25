using Microsoft.EntityFrameworkCore;
using IdentityUserService.Data;

namespace IdentityUserService.Services;

public class UserService(ILogger<UserService> logger,
    AppDbContext appDbContext) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly AppDbContext _dbContext = appDbContext;


    public async Task ConfirmMailingAsync(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
        {
            _logger.LogError("User with id: {id} not found", id);
            return;
        }

        user.CanReceiveMessages = true;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Mailing confirmed for user: {id}", id);
    }
}
