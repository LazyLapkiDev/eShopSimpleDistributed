using Microsoft.EntityFrameworkCore;
using NotificationService.API.Data;
using NotificationService.API.Data.Entities;

namespace NotificationService.API.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly NotificationDbContext _notificationDbContext;

    public UserService(ILogger<UserService> logger,
        NotificationDbContext notificationDbContext)
    {
        _logger = logger;
        _notificationDbContext = notificationDbContext;
    }

    public async Task CreateUserAsync(Guid id, string email, bool isNotificationEnabled)
    {
        _logger.LogInformation("Creating a new user with id: {0} and email: {1}", id, email);
        var now = DateTime.UtcNow;
        var user = new User
        {
            Id = id,
            Email = email,
            IsNotificationEnabled = isNotificationEnabled,
            CreateAt = now,
            UpdatedAt = now
        };

        var entry = await _notificationDbContext.Users.AddAsync(user);
        await _notificationDbContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(Guid id, string email, bool isNotificationEnabled)
    {
        _logger.LogInformation("Updating a new user with id: {0}", id);
        var user = await _notificationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            _logger.LogError("User with id: {0} not found", id);
            return;
        }

        var now = DateTime.UtcNow;
        user.IsNotificationEnabled = isNotificationEnabled;
        user.Email = email;
        user.UpdatedAt = now;

        var entry = _notificationDbContext.Users.Update(user);
        await _notificationDbContext.SaveChangesAsync();
    }
}
