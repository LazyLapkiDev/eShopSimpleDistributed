namespace CatalogService.API.Data.Entities;

public class Brand : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
