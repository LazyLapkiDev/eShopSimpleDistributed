using OrdersService.API.Data.Entities;
using OrdersService.API.Data.Enums;
using OrdersService.API.Models;
using System.Linq;

namespace OrdersService.API.Mapping;

public static class Mapper
{
    public static Order MapToOrder(Guid userId, IEnumerable<OrderItemViewModel> orderItems)
    {
        return new()
        {
            UserId = userId,
            OrderItems = [..orderItems.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            })],
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static OrderViewModel MapToOrderViewModel(Order order)
    {
        return new()
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderItems = MapToOrderItemViewModel(order.OrderItems),
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            RejectComment = order.RejectComment
        };
    }

    public static List<OrderItemViewModel> MapToOrderItemViewModel(IEnumerable<OrderItem> orderItems)
    {
        return [.. orderItems.Select(x => new OrderItemViewModel()
        {
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            Price = x.Price
        })];
    }
}
