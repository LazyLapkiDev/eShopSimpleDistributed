namespace SimpleRabbitEventBus.Abstractions;

public interface IEventHandler
{
    public Task HandleAsync(byte[] bytes);
}
