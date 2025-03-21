using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

public record OrderCustomerVerificateEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
}
