using CatalogService.API.Models.Product;

namespace CatalogService.API.Services.Interfaces
{
    public interface IReservationService
    {
        Task ReserveAsync(Guid orderId, IEnumerable<ReservationItemEventModel> items);
        Task DeleteReservationAsync(Guid orderId);
    }
}