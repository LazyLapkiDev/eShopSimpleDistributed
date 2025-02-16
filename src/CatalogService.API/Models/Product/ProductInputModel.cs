namespace CatalogService.API.Models.Product;

public class ProductInputModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public decimal Price { get; set; }
}
