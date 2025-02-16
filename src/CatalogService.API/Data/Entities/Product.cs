namespace CatalogService.API.Data.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public Category Category { get; set; } = null!;
    public Guid CategoryId { get; set; }

    public Brand Brand { get; set; } = null!;
    public Guid BrandId { get; set; }

    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }

    public decimal Price { get; set; }
}