using SimpleRabbitEventBus.Abstractions;

namespace IdentityUserService.IntegrationEvents.Events;

public record UserVerificatedEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; init; }
    public bool Success { get; init; }
}