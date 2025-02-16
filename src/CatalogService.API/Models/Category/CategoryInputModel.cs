namespace CatalogService.API.Models.Category;

public class CategoryInputModel
{
    public required string Name { get; set; }
    public string? Descriptions { get; set; }
    public Guid? ParentId { get; set; }
}
