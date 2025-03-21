using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Events;

public record ProductUpdatedEvent(Guid ProductId, decimal Price, string Name) : IntegrationEvent;
