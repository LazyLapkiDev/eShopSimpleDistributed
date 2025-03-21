using CatalogService.API.IntegrationEvents.Events;
using CatalogService.API.Services.Interfaces;
using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Handlers;

public class OrderItemsReserveEventHandler(IReservationService reservationService) : IEventHandler<OrderItemsReserveEvent>
{
    public async Task HandleAsync(OrderItemsReserveEvent @event)
    {
        await reservationService.ReserveAsync(@event.OrderId, @event.Items);
    }
}
