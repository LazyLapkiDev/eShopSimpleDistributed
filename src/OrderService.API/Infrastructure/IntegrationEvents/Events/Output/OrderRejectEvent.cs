using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

public record OrderRejectEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}
