using CatalogService.API.Models.Brand;

namespace CatalogService.API.Models.Product;

public class ProductListViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public BrandViewModel Brand { get; set; } = null!;
    public decimal Price { get; set; }
}
