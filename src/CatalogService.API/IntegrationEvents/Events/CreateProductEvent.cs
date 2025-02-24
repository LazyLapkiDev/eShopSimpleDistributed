using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Events;

public record CreateProductEvent : IntegrationEvent
{
    public Guid ProductId { get; init; }
    public decimal Price { get; init; }
    public required string Name { get; init; }
}
