using OrderService.API.Data.Enums;

namespace OrderService.API.Data.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; } = [];
    public string? RejectComment { get; set; }
}
