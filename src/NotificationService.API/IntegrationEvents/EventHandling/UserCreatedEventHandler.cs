using SimpleRabbitEventBus.Abstractions;
using System.Text.Json;
using System.Text;
using NotificationService.API.IntegrationEvents.Events;
using NotificationService.API.Services;

namespace NotificationService.API.IntegrationEvents.EventHandling;

public class UserCreatedEventHandler : IEventHandler
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

    public async Task HandleAsync(byte[] bytes)
    {
        _logger.LogInformation("Receiving user create message from event bus");
        var message = Encoding.UTF8.GetString(bytes);
        var eventData = JsonSerializer.Deserialize(message, typeof(UserCreatedEvent)) as UserCreatedEvent;
        if(eventData is null)
        {
            _logger.LogInformation("Event data is missing");
            return;
        }
        await _userService.CreateUserAsync(eventData.Id, eventData.Email, eventData.IsNotificationEnabled);
        await _notificationService.SendGreetingAsync(eventData.Id, eventData.Email);
        _logger.LogInformation("New user has been created");
    }
}
