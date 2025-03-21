using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

public record OrderConfirmationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}
