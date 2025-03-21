using OrdersService.API.Models;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;

public record OrderCreatedEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
    public IEnumerable<OrderItemViewModel> Items { get; set; } = [];
    public Guid UserId { get; set; }
}