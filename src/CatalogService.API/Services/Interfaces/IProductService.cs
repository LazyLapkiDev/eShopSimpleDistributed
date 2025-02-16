using CatalogService.API.Models;
using CatalogService.API.Models.Product;

namespace CatalogService.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<Guid> CreateAsync(ProductInputModel model);
        Task<bool> DeleteAsync(Guid id);
        Task<ProductViewModel?> GetAsync(Guid id);
        Task<PaginatedResult<ProductListViewModel>> GetPaginatedAsync(PaginationRequest paginationRequest, string? category, string? brand, string? name);
        Task<bool> ReserveAsync(Guid id, int count);
        Task<bool> UpdateAsync(Guid id, ProductInputModel model);
        Task<bool> UpdateStockAsync(Guid id, int stock, bool isIncrease);
    }
}