using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;

public record ProductCreatedEvent : IntegrationEvent
{
    public Guid ProductId { get; init; }
    public decimal Price { get; init; }
    public required string Name { get; init; }
}
