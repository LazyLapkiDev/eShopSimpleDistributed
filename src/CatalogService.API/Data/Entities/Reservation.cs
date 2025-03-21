namespace CatalogService.API.Data.Entities;

public class Reservation : BaseEntity
{
    public Guid OrderId { get; set; }

    public List<Product> Products { get; set; } = [];
    public List<ProductReservation> ProductReservations { get; set; } = [];
}
