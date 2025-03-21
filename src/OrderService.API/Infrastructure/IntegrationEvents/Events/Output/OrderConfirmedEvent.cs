using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

public record OrderConfirmedEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
