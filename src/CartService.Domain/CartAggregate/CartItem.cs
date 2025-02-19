using System.Numerics;

namespace CartService.Domain.CartAggregate;

public class CartItem
{
    public BigInteger Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
