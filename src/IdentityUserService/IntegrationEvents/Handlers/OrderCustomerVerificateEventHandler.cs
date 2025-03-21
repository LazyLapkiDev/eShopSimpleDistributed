using IdentityUserService.IntegrationEvents.Events;
using IdentityUserService.Services;
using SimpleRabbitEventBus.Abstractions;

namespace IdentityUserService.IntegrationEvents.Handlers;

public class OrderCustomerVerificateEventHandler(ILogger<OrderCustomerVerificateEventHandler> logger,
    IUserService userService,
    IEventBus eventBus) : IEventHandler<OrderCustomerVerificateEvent>
{
    public async Task HandleAsync(OrderCustomerVerificateEvent @event)
    {
        try
        {
            logger.LogInformation("Handle event: {name}", nameof(OrderCustomerVerificateEvent));
            var result = await userService.CheckUserVerificationAsync(@event.UserId);
            var verificatedEvent = new UserVerificatedEvent
            {
                UserId = @event.UserId,
                OrderId = @event.OrderId,
                Success = result
            };
            await eventBus.PublishAsync(verificatedEvent);
        }
        catch(Exception ex)
        {
            logger.LogInformation(ex, "Connot handle evenr: {event}", nameof(OrderCustomerVerificateEvent));
        }
    }
}
