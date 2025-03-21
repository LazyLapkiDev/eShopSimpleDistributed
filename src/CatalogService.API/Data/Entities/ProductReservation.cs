namespace CatalogService.API.Data.Entities;

public class ProductReservation
{
    public Guid ProductId { get; set; }
    public Guid ReservationId { get; set; }
    public Product Product { get; set; } = null!;
    public Reservation Reservation { get; set; } = null!;
    public int Quantity { get; set; }
}
