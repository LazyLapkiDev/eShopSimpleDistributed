using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleRabbitEventBus.Abstractions;
using SimpleRabbitEventBus.Models;
using System;


namespace SimpleRabbitEventBus;

public static class RabbitSimpleEventBusDependencyInjectionExtensions
{
    private const string SectionName = "SimpleRabbitEventBus";

    private class SimpleRabbitEventBusBuilder(IServiceCollection services) : ISimpleRabbitEventBusBuilder
    {
        public IServiceCollection Services => services;
    }

    public static ISimpleRabbitEventBusBuilder AddSimpleEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SimpleRabbitEventBusOptions>(configuration.GetSection(SectionName));
        services.AddSingleton<IEventBus, RabbitSimpleEventBus>();
        services.AddHostedService(sp => (RabbitSimpleEventBus)sp.GetRequiredService<IEventBus>());

        return new SimpleRabbitEventBusBuilder(services);
    }

    public static ISimpleRabbitEventBusBuilder AddSubscription<T, TEventHandler>(this ISimpleRabbitEventBusBuilder builder)
        where T : IntegrationEvent
        where TEventHandler : class, IEventHandler<T>
    {
        builder.Services.AddKeyedTransient<IEventHandler, TEventHandler>(typeof(T));
        builder.Services.Configure<EventBusSubscriptionInfo>( o =>
        {
            o.EventTypes[typeof(T).Name] = typeof(T);
        });
        return builder;
    }
}