using SimpleRabbitEventBus.Abstractions;

namespace OrderService.API.IntegrationEvents.Events;

public record ConfirmOrderEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
