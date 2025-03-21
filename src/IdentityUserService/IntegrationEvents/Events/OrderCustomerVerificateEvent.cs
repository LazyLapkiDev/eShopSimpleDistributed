using SimpleRabbitEventBus.Abstractions;

namespace IdentityUserService.IntegrationEvents.Events;

public record OrderCustomerVerificateEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
