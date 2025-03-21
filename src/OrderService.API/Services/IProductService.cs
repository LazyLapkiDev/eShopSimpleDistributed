
namespace OrdersService.API.Services
{
    public interface IProductService
    {
        Task CreateProductAsync(Guid productId, decimal price, string name);
        Task UpdateProductAsync(Guid productId, decimal price, string name);
    }
}