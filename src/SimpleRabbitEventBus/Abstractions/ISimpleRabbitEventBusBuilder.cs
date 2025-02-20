using Microsoft.Extensions.DependencyInjection;

namespace SimpleRabbitEventBus.Abstractions;

public interface ISimpleRabbitEventBusBuilder
{
    public IServiceCollection Services { get; }
}
