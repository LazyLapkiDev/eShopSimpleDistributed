namespace CatalogService.API.Models.Product;

public class ProductStockInputModel
{
    public Guid ProductId { get; set; }
    public int Count { get; set; }
    public bool IsIncrease { get; set; }
}
