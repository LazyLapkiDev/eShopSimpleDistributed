using OrdersService.API.Data.Entities;
using OrdersService.API.Data.Enums;
using OrdersService.API.Models;

namespace OrdersService.API.Services
{
    public interface IOrderService
    {
        Task<Result<Order>> CreateOrderAsync(Order order);
        Task<Result<Order>> GetOrderAsync(Guid id);
        Task<PaginatedResult<Order>> GetOrdersAsync(PaginationRequest paginationRequest);

        Task CancelOrderAsync(Guid id);
        Task ConfirmOrderAsync(Guid id);
    }
}