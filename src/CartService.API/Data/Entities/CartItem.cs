namespace CartService.API.Data.Entities;

public class CartItem : Entity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
