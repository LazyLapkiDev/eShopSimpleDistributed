using OrdersService.API.Data.Enums;

namespace OrdersService.API.Data.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public string? RejectComment { get; set; }
}
