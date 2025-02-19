using CartService.Domain.CartAggregate;

namespace CartService.Domain.Cart;

public class Cart : Entity
{
    public Guid UserId { get; set; }
    public List<CartItem> CartItems { get; set; } = [];

    public decimal TotalPrice { get; set; }
}
