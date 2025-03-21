namespace OrdersService.API.Data.Entities;

public class OrderItem : BaseEntity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
