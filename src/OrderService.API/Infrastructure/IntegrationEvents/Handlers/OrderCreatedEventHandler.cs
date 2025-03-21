using OrdersService.API.Data;
using OrdersService.API.Data.Entities;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure.IntegrationEvents.Handlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, 
    OrderDbContext orderDbContext, 
    OrderSagaOrchestrator orderSagaOrchestrator) : IEventHandler<OrderCreatedEvent>
{
    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        using var transaction = await orderDbContext.Database.BeginTransactionAsync();
        try
        {
            var sagaContext = new OrderSagaContext
            {
                OrderId = @event.OrderId,
                OrderItems = [.. @event.Items.Select(x => new SagaOrderItem { ProductId = x.ProductId, Quantity = x.Quantity })],
                UserId = @event.UserId,
                State = Data.Enums.OrderSagaStatus.Created,
                Step = 0
            };

            await orderDbContext.OrderSagaContexts.AddAsync(sagaContext);
            await orderDbContext.SaveChangesAsync();
            await orderSagaOrchestrator.ProcessAsync(@event.OrderId, orderDbContext);
            await transaction.CommitAsync();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error while handling event: {eventName} for order: {orderId}", nameof(OrderCreatedEvent), @event.OrderId);
            await transaction.RollbackAsync();
        }
        
    }
}
