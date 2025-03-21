using Microsoft.EntityFrameworkCore;
using OrdersService.API.Data;
using OrdersService.API.Data.Entities;
using OrdersService.API.Data.Enums;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;
using OrdersService.API.Models;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly OrderDbContext _orderDbContext;
    private readonly IEventBus _eventBus;

    public OrderService(ILogger<OrderService> logger,
        OrderDbContext orderDbContext,
        IEventBus eventBus)
    {
        _logger = logger;
        _orderDbContext = orderDbContext;
        _eventBus = eventBus;
    }

    public async Task<Result<Order>> CreateOrderAsync(Order order)
    {
        _logger.LogInformation("Creating a new order for user: {userId}", order.UserId);
        using var transaction = await _orderDbContext.Database.BeginTransactionAsync();
        try
        {
            var entry = await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = entry.Entity.Id,
                Items = [.. entry.Entity.OrderItems.Select(p => new OrderItemViewModel { ProductId = p.ProductId, Quantity = p.Quantity })],
                UserId = order.UserId
            };
            await _eventBus.PublishAsync(orderCreatedEvent);
            await transaction.CommitAsync();
            _logger.LogInformation("Order created: {id} for user: {userId}", entry.Entity.Id, entry.Entity.UserId);
            return new(entry.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order for user: {userId}", order.UserId);
            await transaction.RollbackAsync();
            return new(string.Format("Failed to create order for user: {0}", order.UserId));
        }
    }

    public async Task<Result<Order>> GetOrderAsync(Guid id)
    {
        _logger.LogInformation("Getting an order with id: {id}", id);
        var order = await _orderDbContext.Orders.Include(o => o.OrderItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order is null)
        {
            return new(false, data: null);
        }

        return new(order);
    }

    public async Task<PaginatedResult<Order>> GetOrdersAsync(PaginationRequest paginationRequest)
    {
        _logger.LogInformation("Getting a list of orders");

        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var query = _orderDbContext.Orders.Include(o => o.OrderItems)
            .AsNoTrackingWithIdentityResolution();

        var rowsCount = await query.LongCountAsync();

        var result = await query.Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new(pageIndex, pageSize, rowsCount, result);
    }

    public async Task CancelOrderAsync(Guid id)
    {
        using var transaction = await _orderDbContext.Database.BeginTransactionAsync();
        try
        {
            await ChangeOrderStatusAsync(id, OrderStatus.Canceled);
            var cancelledEvent = new OrderCancelledEvent
            {
                OrderId = id
            };
            await _eventBus.PublishAsync(cancelledEvent);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Connot cancel the order: {id}", id);
        }
        
    }

    public async Task ConfirmOrderAsync(Guid id)
    {
        using var transaction = await _orderDbContext.Database.BeginTransactionAsync();
        try
        {
            await ChangeOrderStatusAsync(id, OrderStatus.Confirmed);
            var confirmedEvent = new OrderConfirmedEvent
            {
                OrderId = id
            };
            await _eventBus.PublishAsync(confirmedEvent);
            await transaction.CommitAsync();
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Connot confirm the order: {id}", id);
        }
    }

    private async Task ChangeOrderStatusAsync(Guid id, OrderStatus newStatus)
    {
        var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);

        if (order is null)
        {
            return;
        }

        order.Status = newStatus;
        _orderDbContext.Orders.Update(order);
        await _orderDbContext.SaveChangesAsync();
    }
}
