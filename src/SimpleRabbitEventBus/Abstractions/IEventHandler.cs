namespace SimpleRabbitEventBus.Abstractions;

public interface IEventHandler
{
    Task HandleAsync(IntegrationEvent @event);
}

public interface IEventHandler<T> : IEventHandler 
    where T : IntegrationEvent
{
    Task HandleAsync(T @event);

    Task IEventHandler.HandleAsync(IntegrationEvent @event) => HandleAsync((T)@event);
}
