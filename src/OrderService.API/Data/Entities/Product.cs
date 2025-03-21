namespace OrdersService.API.Data.Entities;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
