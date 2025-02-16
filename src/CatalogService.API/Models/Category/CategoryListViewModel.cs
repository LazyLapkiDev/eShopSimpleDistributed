namespace CatalogService.API.Models.Category;

public class CategoryListViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public int ProductsCount { get; set; }
}
