using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents;

public record OrderSagaIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
    //Чтобы не делать по два события на каждый шаг
    public bool Success { get; init; }
}
