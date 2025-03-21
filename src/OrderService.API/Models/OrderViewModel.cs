using OrdersService.API.Data.Enums;

namespace OrdersService.API.Models;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<OrderItemViewModel> OrderItems { get; set; } = [];
    public string? RejectComment { get; set; }
    public DateTime CreatedAt { get; set; }
}
