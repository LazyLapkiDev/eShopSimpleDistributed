using CatalogService.API.Models.Brand;

namespace CatalogService.API.Services.Interfaces
{
    public interface IBrandService
    {
        Task<BrandViewModel> CreateAsync(BrandInputModel model);
        Task<bool> DeleteAsync(Guid id);
        Task<List<BrandViewModel>> GetAsync();
        Task<BrandViewModel?> GetAsync(Guid id);
        Task<BrandViewModel?> UpdateAsync(Guid id, BrandInputModel model);
    }
}