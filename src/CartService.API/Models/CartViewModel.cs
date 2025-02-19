namespace CartService.API.Models;

public class CartViewModel
{
    public Guid UserId { get; set; }
    public IEnumerable<CartItemViewModel> Items { get; set; } = [];
    public decimal TotalPrice { get; set; }
}

public class CartItemViewModel
{
    public Guid ProductId { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}
