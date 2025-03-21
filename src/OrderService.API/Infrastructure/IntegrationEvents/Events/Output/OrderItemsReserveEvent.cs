using OrdersService.API.Models;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

public record OrderItemsReserveEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public IEnumerable<OrderItemViewModel> Items { get; set; } = [];
}
