namespace SimpleRabbitEventBus.Abstractions;

public abstract class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }
    //public string? EventType { get; set; }
    public string? CorrelationId { get; set; }
}