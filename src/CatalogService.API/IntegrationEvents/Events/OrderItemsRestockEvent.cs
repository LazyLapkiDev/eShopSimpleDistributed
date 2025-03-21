using CatalogService.API.Models.Product;
using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Events;

public record OrderItemsRestockEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public IEnumerable<ReservationItemEventModel> Items { get; set; } = [];
}
