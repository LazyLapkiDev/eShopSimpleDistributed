namespace SimpleRabbitEventBus.Abstractions;

public interface IEventBus
{
    public Task PublishAsync(dynamic integrationEvent, CancellationToken cancellationToken = default);
}
