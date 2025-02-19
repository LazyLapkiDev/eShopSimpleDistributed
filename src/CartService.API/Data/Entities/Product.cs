namespace CartService.API.Data.Entities;

public class Product : Entity
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
