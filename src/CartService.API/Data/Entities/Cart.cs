namespace CartService.API.Data.Entities;

public class Cart : Entity
{
    public Guid UserId { get; set; }
    public IEnumerable<CartItem> Items { get; set; } = [];
}
