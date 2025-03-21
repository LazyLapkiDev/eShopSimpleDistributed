using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

public record OrderCancelledEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
}
