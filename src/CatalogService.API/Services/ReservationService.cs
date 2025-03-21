using CatalogService.API.Data;
using CatalogService.API.Data.Entities;
using CatalogService.API.IntegrationEvents.Events;
using CatalogService.API.Models.Product;
using CatalogService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.Services;

public class ReservationService : IReservationService
{
    private readonly ILogger<ReservationService> _logger;
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IEventBus _eventBus;

    public ReservationService(ILogger<ReservationService> logger,
        CatalogDbContext catalogDbContext,
        IEventBus eventBus)
    {
        _logger = logger;
        _catalogDbContext = catalogDbContext;
        _eventBus = eventBus;
    }

    public async Task ReserveAsync(Guid orderId, IEnumerable<ReservationItemEventModel> items)
    {
        using var transaction = await _catalogDbContext.Database.BeginTransactionAsync();
        try
        {
            _logger.LogInformation("Reserve products for order: {order}", orderId);
            var reservation = new Reservation
            {
                OrderId = orderId,
                ProductReservations = items.Select(x => new ProductReservation { ProductId = x.ProductId, Quantity = x.Quantity }).ToList()
            };
            await _catalogDbContext.Reservations.AddAsync(reservation);
            await _catalogDbContext.SaveChangesAsync();
            var reservedEvent = new ProductsReservedEvent(orderId, true);
            await _eventBus.PublishAsync(reservedEvent);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reserve products for order: {order}", orderId);
            await transaction.RollbackAsync();
        }
    }

    public async Task DeleteReservationAsync(Guid orderId)
    {
        _logger.LogInformation("Delete reservation products for order: {order}", orderId);
        var reservation = await _catalogDbContext.Reservations.FirstOrDefaultAsync(x => x.OrderId == orderId);
        if(reservation is null)
        {
            _logger.LogInformation("Reservation products for order: {order} not found", orderId);
            return;
        }
        _catalogDbContext.Reservations.Remove(reservation);
        await _catalogDbContext.SaveChangesAsync();
    }
}
