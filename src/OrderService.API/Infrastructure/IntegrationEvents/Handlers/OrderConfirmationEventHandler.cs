using OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;
using OrdersService.API.Services;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class OrderConfirmationEventHandler(IOrderService orderService) : IEventHandler<OrderConfirmationEvent>
{
    public async Task HandleAsync(OrderConfirmationEvent @event)
    {
        await orderService.ConfirmOrderAsync(@event.OrderId);
    }
}