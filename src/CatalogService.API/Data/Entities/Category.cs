namespace CatalogService.API.Data.Entities;

public class Category : BaseEntity
{
    public required string Name { get; set; }
    public string? Descriptions { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Product> Products { get; set; } = [];

}
