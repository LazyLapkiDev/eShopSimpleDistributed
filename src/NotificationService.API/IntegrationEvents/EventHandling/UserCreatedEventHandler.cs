using SimpleRabbitEventBus.Abstractions;
using System.Text.Json;
using System.Text;
using NotificationService.API.IntegrationEvents.Events;

namespace NotificationService.API.IntegrationEvents.EventHandling;

public class UserCreatedEventHandler : IEventHandler
{
    public Task HandleAsync(byte[] bytes)
    {
        var message = Encoding.UTF8.GetString(bytes);
        var eventData = JsonSerializer.Deserialize(message, typeof(UserCreatedEvent)) as UserCreatedEvent;
        Console.WriteLine($" [x] Received {eventData?.Email}");
        return Task.CompletedTask;
    }
}
