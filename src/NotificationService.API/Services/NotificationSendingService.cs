using NotificationService.API.Data;
using NotificationService.API.Data.Entities;

namespace NotificationService.API.Services;

public class NotificationSendingService : INotificationService
{
    private readonly ILogger<NotificationSendingService> _logger;
    private readonly NotificationDbContext _notificationDbContextz;

    public NotificationSendingService(ILogger<NotificationSendingService> logger, NotificationDbContext notificationDbContext)
    {
        _logger = logger;
        _notificationDbContextz = notificationDbContext;
    }

    public async Task SendGreetingAsync(Guid userId, string email)
    {
        _logger.LogInformation("Creating a welcome notification for user: {0} with email: {1}", userId, email);
        var now = DateTime.UtcNow;
        Notification notification = new()
        {
            Subject = "Welcome",
            Body = "",
            UserId = userId,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _notificationDbContextz.Notifications.AddAsync(notification);
        await _notificationDbContextz.SaveChangesAsync();
        _logger.LogInformation("Welcome notification sent to {0}", email);
    }
}
