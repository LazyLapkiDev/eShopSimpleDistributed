using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using OrdersService.API.Services;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class ProductUpdatedEventHandler(IProductService productService) : IEventHandler<ProductUpdatedEvent>
{
    public async Task HandleAsync(ProductUpdatedEvent @event)
    {
        await productService.UpdateProductAsync(@event.ProductId, @event.Price, @event.Name);
    }
}
