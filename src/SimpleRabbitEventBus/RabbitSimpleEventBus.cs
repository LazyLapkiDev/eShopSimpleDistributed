using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleRabbitEventBus.Abstractions;
using SimpleRabbitEventBus.Models;
using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace SimpleRabbitEventBus;

public class RabbitSimpleEventBus : IHostedService,IEventBus, IDisposable, IAsyncDisposable
{
    private const string EXCHANGE = "eShopSimple";
    private bool _isDisposed = false;
    private IConnection _connection = null!;
    private IChannel _channel = null!;

    private readonly Dictionary<string, IEventHandler>? _events;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitSimpleEventBus> _logger;
    private readonly SimpleRabbitEventBusOptions _options;
    private readonly EventBusSubscriptionInfo _subscriptionOptions;

    public RabbitSimpleEventBus(IServiceProvider serviceProvider, 
        ILogger<RabbitSimpleEventBus> logger,
        IOptions<SimpleRabbitEventBusOptions> options,
        IOptions<EventBusSubscriptionInfo> subscriptionOptions)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
        _subscriptionOptions = subscriptionOptions.Value;
    }

    public async Task PublishAsync(dynamic integrationEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventName = (integrationEvent as IntegrationEvent)?.GetType().Name ?? throw new Exception("Event name is required");
            await _channel.ExchangeDeclareAsync(exchange: EXCHANGE, type: ExchangeType.Direct, cancellationToken: cancellationToken);
            var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent) as byte[];
            await _channel.BasicPublishAsync(EXCHANGE, eventName, body, cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error with SimpleRabbitEventBus publish method");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName ?? "localhost"
            };

            if (!string.IsNullOrEmpty(_options.UserName))
            {
                factory.UserName = _options.UserName;
            }

            if (!string.IsNullOrEmpty(_options.Password))
            {
                factory.Password = _options.Password;
            }

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            _channel.CallbackExceptionAsync += async (sender, ea) =>
            {
                _logger.LogError(ea.Exception, "Error with RabbitMQ consumer channel");
                await Task.CompletedTask;
            };

            //await _channel.ExchangeDeclareAsync(EXCHANGE,
            //    type: ExchangeType.Direct,
            //    durable: true,
            //    autoDelete: false,
            //    arguments: null,
            //    noWait: false,
            //    cancellationToken: cancellationToken);

            await _channel.ExchangeDeclareAsync(exchange: EXCHANGE, type: ExchangeType.Direct, cancellationToken: cancellationToken);

            var queueDeclareResult = await _channel.QueueDeclareAsync();
            var queueName = queueDeclareResult.QueueName;
            foreach (var @event in _subscriptionOptions.EventTypes.Keys)
            {
                await _channel.QueueBindAsync(queueName, EXCHANGE, @event);
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var success = _subscriptionOptions.EventTypes.TryGetValue(ea.RoutingKey, out var t);
                if (!success)
                {
                    _logger.LogWarning("Unable to resolve event type for event name {EventName}", ea.RoutingKey);
                    return;
                }
                var body = ea.Body.ToArray();
                await ProcessEvent(body, t!);
                //await Parallel.ForEachAsync(handlers, async (handler, cancellationToken) =>
                //{
                //    await handler.HandleAsync(body);
                //});
            };

            _logger.LogInformation("Starting RabbitMQ basic consume");
            await _channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
        }
        catch
        {
            _logger.LogError("Cannot connect to RabbitMQ");
        }
    }

    private async Task ProcessEvent(byte[] body, Type eventType)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(eventType);
            await using var scope = _serviceProvider.CreateAsyncScope();
            var handlers = scope.ServiceProvider.GetKeyedServices<IEventHandler>(eventType);
            var @event = DeserializeMessage(body, eventType);
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(body);
            }
            //await Parallel.ForEachAsync(handlers, async (handler, cancellationToken) =>
            //{
            //    await handler.HandleAsync(body);
            //});
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unable to process message for event name {EventName}", nameof(eventType));
        }
    }

    private IntegrationEvent? DeserializeMessage(byte[] bytes, Type eventType)
    {
        return JsonSerializer.Deserialize(bytes, eventType) as IntegrationEvent;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

    public void Dispose()
    {
        if(!_isDisposed)
        {
            _channel.Dispose();
            _connection?.Dispose();
            _isDisposed = true;
        }
        
    }

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            await _channel.DisposeAsync();
            await _connection.DisposeAsync();
            _isDisposed = true;
        }
    }
}
