namespace CatalogService.API.Models.Brand;

public class BrandViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
