using CatalogService.API.Data.Entities;
using CatalogService.API.Models.Category;
using CatalogService.API.Models.Brand;

namespace CatalogService.API.Models.Product;

public class ProductViewModel : BaseViewModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public required CategoryViewModel Category { get; set; }

    public required BrandViewModel Brand { get; set; } 

    public int AvailableQuantity { get; set; }

    public decimal Price { get; set; }
}
