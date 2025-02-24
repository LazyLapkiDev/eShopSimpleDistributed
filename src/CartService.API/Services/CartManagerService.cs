using CartService.API.Data;
using CartService.API.Data.Entities;
using CartService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CartService.API.Services;

public class CartManagerService
{
    private readonly ILogger<CartManagerService> _logger;
    private readonly CartDbContext _cartDbContext;

    public CartManagerService(ILogger<CartManagerService> logger,
        CartDbContext cartDbContext)
    {
        _logger = logger;
        _cartDbContext = cartDbContext;
    }

    public async Task<CartViewModel> GetAsync(Guid userId)
    {
        _logger.LogInformation("Getting the cart for user: {0}", userId);
        var cart = await _cartDbContext.Carts.Include(c => c.Items)
            .ThenInclude(ct => ct.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if(cart is null)
        {
            _logger.LogWarning("No cart for user: {0}", userId);
            cart = new Cart { UserId = userId };
            await _cartDbContext.Carts.AddAsync(cart);
            await _cartDbContext.SaveChangesAsync();
        }

        decimal totalPrice = 0;

        return new CartViewModel
        { 
            UserId = userId,
            Items = cart.Items.Select(c =>
            {
                var total = c.Quantity * c.Product.Price;
                totalPrice += total;
                return new CartItemViewModel
                {
                    ProductId = c.ProductId,
                    Name = c.Product.Name,
                    Price = c.Product.Price,
                    Quantity = c.Quantity,
                    Total = total
                };
            }),
            TotalPrice = totalPrice
        };
    }

    public async Task UpdateAsync(Guid userId, CartItemInputModel[] input)
    {
        _logger.LogInformation("Updating the cart of user: {0}", userId);
        var cart = await _cartDbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if(cart is not null)
        {
            var items = input.Select(x => new CartItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            });
            cart.Items = items;
            _cartDbContext.Carts.Update(cart);
            await _cartDbContext.SaveChangesAsync();
            return;
        }

        _logger.LogWarning("No cart for user: {0}", userId);
        cart = new Cart
        {
            UserId = userId,
            Items = input.Select(x => new CartItem { ProductId = x.ProductId, Quantity = x.Quantity })
        };
        await _cartDbContext.Carts.AddAsync(cart);
        await _cartDbContext.SaveChangesAsync();
    }

    public async Task CleanAsync(Guid userId)
    {
        var cart = await _cartDbContext.Carts.FirstAsync(c => c.UserId == userId);

        cart.Items = [];
        _cartDbContext.Carts.Update(cart);
        await _cartDbContext.SaveChangesAsync();
    }
}
