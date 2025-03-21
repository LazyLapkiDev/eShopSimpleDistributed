using OrdersService.API.Data;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class UserVerificatedEventHandler(OrderSagaOrchestrator orderSagaOrchestrator,
    OrderDbContext orderDbContext) : IEventHandler<UserVerificatedEvent>
{
    public async Task HandleAsync(UserVerificatedEvent @event)
    {
        await orderSagaOrchestrator.ProcessAsync(@event.OrderId, orderDbContext);
    }
}
