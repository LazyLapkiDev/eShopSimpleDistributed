using SimpleRabbitEventBus.Abstractions;

namespace OrderService.API.IntegrationEvents.Events;

public record CreateOrderEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public decimal Price { get; set; }
    public IEnumerable<OrderItem> Items { get; set; } = [];
}

public record OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
