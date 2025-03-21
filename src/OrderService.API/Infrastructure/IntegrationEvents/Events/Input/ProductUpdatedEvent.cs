using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;

public record ProductUpdatedEvent(Guid ProductId, decimal Price, string Name) : IntegrationEvent;