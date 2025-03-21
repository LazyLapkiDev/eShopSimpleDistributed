using OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;
using OrdersService.API.Services;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class OrderRejectEventHandler(IOrderService orderService) : IEventHandler<OrderRejectEvent>
{
    public async Task HandleAsync(OrderRejectEvent @event)
    {
        await orderService.CancelOrderAsync(@event.OrderId);
    }
}
