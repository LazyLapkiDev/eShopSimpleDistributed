using SimpleRabbitEventBus.Abstractions;
using NotificationService.API.IntegrationEvents.Events;
using NotificationService.API.Services;

namespace NotificationService.API.IntegrationEvents.EventHandling;

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, 
        IUserService userService,
        INotificationService notificationService)
    {
        _logger = logger;
        _userService = userService;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(UserCreatedEvent @event)
    {
        _logger.LogInformation("Receiving user create message from event bus");
        await _userService.CreateUserAsync(@event.UserId, @event.Email, @event.IsNotificationEnabled);
        await _notificationService.SendGreetingAsync(@event.UserId, @event.Email);
        _logger.LogInformation("New user has been created");
    }
}
