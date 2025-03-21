using Microsoft.EntityFrameworkCore;
using IdentityUserService.Data;
using SimpleRabbitEventBus.Abstractions;

namespace IdentityUserService.Services;

public class UserService(ILogger<UserService> logger,
    AppDbContext appDbContext, 
    IEventBus eventBus) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly AppDbContext _dbContext = appDbContext;
    private readonly IEventBus _eventBus = eventBus;


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

    public async Task<bool> CheckUserVerificationAsync(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
        {
            _logger.LogError("User with id: {id} not found", id);
            return false;
        }

        return user.CanReceiveMessages;
    }
}
