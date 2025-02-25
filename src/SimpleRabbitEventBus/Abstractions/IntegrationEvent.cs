namespace SimpleRabbitEventBus.Abstractions;

public abstract record IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
        EventType = this.GetType().Name;
    }

    public Guid Id { private get; init; }
    public Guid GetIntegrationEventId() => Id;

    public DateTime CreationDate { get; init; }
    public string? EventType { get; init; }
    public string? CorrelationId { get; init; }
}