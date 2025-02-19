namespace CartService.API.Models;

public class CartItemInputModel
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}