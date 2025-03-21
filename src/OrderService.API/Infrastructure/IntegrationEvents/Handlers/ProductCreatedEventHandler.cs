using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using OrdersService.API.Services;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class ProductCreatedEventHandler(IProductService productService) : IEventHandler<ProductCreatedEvent>
{
    public async Task HandleAsync(ProductCreatedEvent @event)
    {
        await productService.CreateProductAsync(@event.ProductId, @event.Price, @event.Name);
    }
}
