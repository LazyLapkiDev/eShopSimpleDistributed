using SimpleRabbitEventBus.Abstractions;
using IdentityUserService.Services;
using IdentityUserService.IntegrationEvents.Events;

namespace IdentityUserService.IntegrationEvents.Handlers;

public class NotificationUserCreatedConfirmEventHandler : IEventHandler<NotificationUserCreatedConfirmEvent>
{
    private readonly ILogger<NotificationUserCreatedConfirmEventHandler> _logger;
    private readonly IUserService _userService;

    public NotificationUserCreatedConfirmEventHandler(ILogger<NotificationUserCreatedConfirmEventHandler> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task HandleAsync(NotificationUserCreatedConfirmEvent @event)
    {
        _logger.LogInformation("Receiving user create message from event bus");
        await _userService.ConfirmMailingAsync(@event.UserId);
    }
}
