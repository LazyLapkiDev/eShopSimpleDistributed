namespace CatalogService.API.Models.Category;

public class CategoryViewModel : BaseViewModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
