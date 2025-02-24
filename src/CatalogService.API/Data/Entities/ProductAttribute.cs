namespace CatalogService.API.Data.Entities;

public class ProductAttribute : BaseEntity
{
    public required string Name { get; set; }

    public IEnumerable<Product> Products { get; set; } = [];
}
