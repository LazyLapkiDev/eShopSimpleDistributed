using OrdersService.API.Data;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class ProductsReservedEventHandler(OrderSagaOrchestrator orderSagaOrchestrator,
    OrderDbContext orderDbContext) : IEventHandler<ProductsReservedEvent>
{
    public async Task HandleAsync(ProductsReservedEvent @event)
    {
        await orderSagaOrchestrator.ProcessAsync(@event.OrderId, orderDbContext);
    }
}
