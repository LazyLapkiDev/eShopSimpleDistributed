using CatalogService.API.IntegrationEvents.Events;
using CatalogService.API.Services.Interfaces;
using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Handlers;

public class OrderItemsRestockEventHandler(IReservationService reservationService) : IEventHandler<OrderItemsRestockEvent>
{
    public async Task HandleAsync(OrderItemsRestockEvent @event)
    {
        await reservationService.DeleteReservationAsync(@event.OrderId);
    }
}
