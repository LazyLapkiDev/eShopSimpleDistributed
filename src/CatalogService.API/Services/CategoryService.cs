using CatalogService.API.Data;
using CatalogService.API.Data.Entities;
using CatalogService.API.Models.Category;
using CatalogService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly CatalogDbContext _catalogContext;

    public CategoryService(ILogger<CategoryService> logger,
        CatalogDbContext catalogContext)
    {
        _logger = logger;
        _catalogContext = catalogContext;
    }

    public async Task<List<CategoryListViewModel>> GetAllCategoriesAsync()
    {
        _logger.LogInformation("Getting list of all categories");
        var categories = await _catalogContext.Categories.Where(x => x.IsDeleted != false)
            .Select(x => new CategoryListViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Descriptions ?? string.Empty,
                ProductsCount = x.Products.Count,
            })
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        return categories;
    }

    public async Task<CategoryViewModel?> GetCategoryAsync(Guid categoryId)
    {
        _logger.LogInformation("Getting detailed category with id: {0}", categoryId);
        var category = await _catalogContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsDeleted != false && x.Id == categoryId);

        if(category is null)
        {
            return null;
        }

        return new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Descriptions ?? string.Empty,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public async Task<CategoryViewModel> CreateAsync(CategoryInputModel categoryModel)
    {
        _logger.LogInformation("Creating a new category with name: {0}", categoryModel.Name);
        Category category = new ()
        { 
            Name = categoryModel.Name,
            Descriptions = categoryModel.Descriptions,
            ParentId = categoryModel.ParentId
        };

        var entry = await _catalogContext.Categories.AddAsync(category);
        await _catalogContext.SaveChangesAsync();
        return new CategoryViewModel
        {
            Id = entry.Entity.Id,
            Name = entry.Entity.Name,
            Description = entry.Entity.Descriptions ?? string.Empty,
            CreatedAt = entry.Entity.CreatedAt,
            UpdatedAt = entry.Entity.UpdatedAt
        };
    }

    public async Task<CategoryViewModel?> UpdateAsync(Guid id, CategoryInputModel categoryModel)
    {
        var category = await _catalogContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if(category is null)
        {
            return null;
        }

        category.Name = categoryModel.Name;
        category.Descriptions = categoryModel.Descriptions;
        category.ParentId = categoryModel.ParentId;

        var entry = _catalogContext.Categories.Update(category);
        await _catalogContext.SaveChangesAsync();

        return new CategoryViewModel
        {
            Id = entry.Entity.Id,
            Name = entry.Entity.Name,
            Description = entry.Entity.Descriptions ?? string.Empty,
            CreatedAt = entry.Entity.CreatedAt,
            UpdatedAt = entry.Entity.UpdatedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _catalogContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is null)
        {
            return false;
        }
        category.IsDeleted = true;
        var entry = _catalogContext.Categories.Update(category);
        await _catalogContext.SaveChangesAsync();
        return true;
    }
}
