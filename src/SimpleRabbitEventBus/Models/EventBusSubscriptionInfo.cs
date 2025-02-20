namespace SimpleRabbitEventBus.Models;

public class EventBusSubscriptionInfo
{
    public Dictionary<string, Type> EventTypes { get; } = [];
}
