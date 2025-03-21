using Microsoft.EntityFrameworkCore;
using OrdersService.API.Data;
using OrdersService.API.Data.Entities;
using OrdersService.API.Data.Enums;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;
using OrdersService.API.Models;
using SimpleRabbitEventBus.Abstractions;

namespace OrdersService.API.Infrastructure;

public class OrderSagaOrchestrator
{
    private readonly ILogger<OrderSagaOrchestrator> _logger;
    private readonly IEventBus _eventBus;
    private readonly OrderDbContext _orderDbContext;
    private List<(Func<OrderSagaContext, IEventBus, Task>, Func<OrderSagaContext, IEventBus, Task>)> _steps = [];

    public OrderSagaOrchestrator(ILogger<OrderSagaOrchestrator> logger, IEventBus eventBus, OrderDbContext orderDbContext)
    {
        _logger = logger;
        _eventBus = eventBus;
        _orderDbContext = orderDbContext;

        _steps.Add((VerificationStep, VerificationCompensationStep));
        _steps.Add((ReserveStep, ReserveCompensationStep));
        _steps.Add((ConfirmationStep, ConfirmationCompensationStep));
    }

    public async Task ProcessAsync(Guid orderId, OrderDbContext orderDbContext, bool result = true)
    {
        _logger.LogDebug("Proccess saga step");
        //using var transaction = await orderDbContext.Database.BeginTransactionAsync();
        try
        {
            var context = await orderDbContext.OrderSagaContexts
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (context is null)
            {
                _logger.LogError("Saga context for order: {orderId}  not found", orderId);
                throw new Exception("Saga context not found");
            }

            var (step, compensation) = _steps[context.Step];
            if (result)
            {
                await step.Invoke(context, _eventBus);
                if (context.Step == _steps.Count - 1)
                {
                    context.State = OrderSagaStatus.Completed;
                }
                else
                {
                    context.Step += 1;
                }
            }
            else
            {
                //await compensation.Invoke(context, _eventBus);
                //context.Step -= 1;
                //context.State = OrderSagaStatus.Failed;
                for(var i = context.Step; i >= 0; i--)
                {
                    _steps[i].Item2?.Invoke(context, _eventBus);
                }
                context.State = OrderSagaStatus.Failed;
            }

            await orderDbContext.SaveChangesAsync();
            //await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //await transaction.RollbackAsync();
            _logger.LogError(ex, "Cannot process a step for order: {orderId}", orderId);
        }
    }

    private async Task ReserveStep(OrderSagaContext sagaContext, IEventBus eventBus)
    {
        var reserveEvent = new OrderItemsReserveEvent
        {
            OrderId = sagaContext.OrderId,
            Items = sagaContext.OrderItems.Select(x => new OrderItemViewModel { ProductId = x.ProductId, Quantity = x.Quantity})
        };
        await eventBus.PublishAsync(reserveEvent);
    }

    private async Task ReserveCompensationStep(OrderSagaContext sagaContext, IEventBus eventBus)
    {
        var reserveEvent = new OrderItemsRestockEvent
        {
            OrderId = sagaContext.OrderId,
            Items = sagaContext.OrderItems.Select(x => new OrderItemViewModel { ProductId = x.ProductId, Quantity = x.Quantity })
        };
        await eventBus.PublishAsync(reserveEvent);
    }

    private async Task VerificationStep(OrderSagaContext sagaContext, IEventBus eventBus)
    {
        var reserveEvent = new OrderCustomerVerificateEvent
        {
            OrderId = sagaContext.OrderId,
            UserId = sagaContext.UserId
        };
        await eventBus.PublishAsync(reserveEvent);
    }

    private async Task VerificationCompensationStep(OrderSagaContext sagaContext, IEventBus eventBus)
    {
        var rejectOrder = new OrderRejectEvent
        {
            OrderId = sagaContext.OrderId
        };
        await eventBus.PublishAsync(rejectOrder);
    }

    private async Task ConfirmationStep(OrderSagaContext sagaContext, IEventBus eventBus)
    {
        var confirmEvent = new OrderConfirmationEvent
        {
            OrderId = sagaContext.OrderId
        };
        await eventBus.PublishAsync(confirmEvent);
    }

    private async Task ConfirmationCompensationStep(OrderSagaContext sagaContext, IEventBus eventBus)
    {
        //No compensation needed
        await Task.CompletedTask;
    }
}
