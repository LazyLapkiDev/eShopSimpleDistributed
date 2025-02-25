namespace SimpleRabbitEventBus.Exceptions;

public class EventNullException : Exception
{
    public string EventType { get; init; }
    public EventNullException(string eventType, string message)
        : base(message)
    {
        EventType = eventType;
    }
}
