using Microsoft.EntityFrameworkCore;
using OrdersService.API.Data;
using OrdersService.API.Data.Entities;

namespace OrdersService.API.Services;

public class ProductService(OrderDbContext orderDbContext) : IProductService
{
    public async Task CreateProductAsync(Guid productId, decimal price, string name)
    {
        Product product = new Product()
        {
            ProductId = productId,
            Name = name,
            Price = price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await orderDbContext.Products.AddAsync(product);
        await orderDbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Guid productId, decimal price, string name)
    {
        var product = await orderDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product is null)
        {
            return;
        }
        product.Name = name;
        product.Price = price;

        orderDbContext.Products.Update(product);
        await orderDbContext.SaveChangesAsync();
    }
}
