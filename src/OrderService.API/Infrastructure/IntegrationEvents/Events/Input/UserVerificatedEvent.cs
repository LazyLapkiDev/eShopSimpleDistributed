namespace OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;

public record UserVerificatedEvent : OrderSagaIntegrationEvent
{
    public Guid UserId { get; set; }
}
