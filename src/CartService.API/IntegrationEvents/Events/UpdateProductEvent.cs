using SimpleRabbitEventBus.Abstractions;

namespace CartService.API.IntegrationEvents.Events;

public record UpdateProductEvent : IntegrationEvent
{
    public Guid ProductId { get; init; }
    public decimal Price { get; init; }
    public required string Name { get; init; }
}
