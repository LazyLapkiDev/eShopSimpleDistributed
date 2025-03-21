using OrdersService.API.Data.Enums;

namespace OrdersService.API.Data.Entities;

public class OrderSagaContext
{
    public Guid OrderId { get; set; }
    public int Step { get; set; }
    public OrderSagaStatus State { get; set; }
    public ICollection<SagaOrderItem> OrderItems { get; set; } = null!;
    public Guid UserId { get; set; }
}


public class SagaOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}