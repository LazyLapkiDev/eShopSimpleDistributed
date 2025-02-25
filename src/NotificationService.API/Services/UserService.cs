using Microsoft.EntityFrameworkCore;
using NotificationService.API.Data;
using NotificationService.API.Data.Entities;
using NotificationService.API.IntegrationEvents.Events;
using SimpleRabbitEventBus.Abstractions;

namespace NotificationService.API.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IEventBus _eventBus;

    public UserService(ILogger<UserService> logger,
        NotificationDbContext notificationDbContext,
        IEventBus eventBus)
    {
        _logger = logger;
        _notificationDbContext = notificationDbContext;
        _eventBus = eventBus;
    }

    public async Task CreateUserAsync(Guid id, string email, bool isNotificationEnabled)
    {
        _logger.LogInformation("Creating a new user with id: {id} and email: {email}", id, email);
        using var transaction = await _notificationDbContext.Database.BeginTransactionAsync();
        try
        {
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

            var confirmEvent = new NotificationUserCreatedConfirmEvent(id, true);
            await _eventBus.PublishAsync(confirmEvent);

            await transaction.CommitAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "User creation failed, id: {id}", id);
            await transaction.RollbackAsync();
        }
    }

    public async Task UpdateUserAsync(Guid id, string email, bool isNotificationEnabled)
    {
        _logger.LogInformation("Updating a new user with id: {id}", id);
        var user = await _notificationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            _logger.LogError("User with id: {id} not found", id);
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
