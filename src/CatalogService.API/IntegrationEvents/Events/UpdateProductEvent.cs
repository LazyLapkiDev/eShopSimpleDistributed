using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Events;

public record UpdateProductEvent(Guid ProductId, decimal Price, string Name) : IntegrationEvent;
