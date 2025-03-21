using SimpleRabbitEventBus.Abstractions;
using IdentityUserService.Services;
using IdentityUserService.IntegrationEvents.Events;

namespace IdentityUserService.IntegrationEvents.Handlers;

public class NotificationUserCreatedConfirmEventHandler(ILogger<NotificationUserCreatedConfirmEventHandler> logger,
        IUserService userService) : IEventHandler<NotificationUserCreatedConfirmEvent>
{
    public async Task HandleAsync(NotificationUserCreatedConfirmEvent @event)
    {
        logger.LogInformation("Receiving user create message from event bus");
        await userService.ConfirmMailingAsync(@event.UserId);
    }
}
