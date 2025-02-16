using CatalogService.API.Models.Category;

namespace CatalogService.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryListViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel?> GetCategoryAsync(Guid categoryId);
        Task<CategoryViewModel> CreateAsync(CategoryInputModel categoryModel);
        Task<CategoryViewModel?> UpdateAsync(Guid id, CategoryInputModel categoryModel);
        Task<bool> DeleteAsync(Guid id);
    }
}