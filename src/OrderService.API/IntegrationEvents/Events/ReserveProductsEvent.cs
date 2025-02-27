using SimpleRabbitEventBus.Abstractions;

namespace OrderService.API.IntegrationEvents.Events;

public record ReserveProductsEvent : IntegrationEvent
{
    public bool Success { get; set; }
}
