using CatalogService.API.Models.Product;
using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Events;

public record OrderItemsReserveEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public IEnumerable<ReservationItemEventModel> Items { get; set; } = [];
}
