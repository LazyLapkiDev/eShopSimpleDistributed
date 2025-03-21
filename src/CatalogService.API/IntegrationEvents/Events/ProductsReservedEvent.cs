using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.IntegrationEvents.Events;

public record ProductsReservedEvent(Guid OrderId, bool Success) : IntegrationEvent
{
}
