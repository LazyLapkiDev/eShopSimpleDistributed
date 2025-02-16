using CatalogService.API.Data;
using CatalogService.API.Data.Entities;
using CatalogService.API.Models.Brand;
using CatalogService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API.Services;

public class BrandService : IBrandService
{
    private readonly ILogger<BrandService> _logger;
    private readonly CatalogDbContext _catalogDbContext;

    public BrandService(ILogger<BrandService> logger,
        CatalogDbContext catalogDbContext)
    {
        _logger = logger;
        _catalogDbContext = catalogDbContext;
    }

    public async Task<List<BrandViewModel>> GetAsync()
    {
        _logger.LogInformation("Getting list of brands");
        return await _catalogDbContext.Brands.Select(b => new BrandViewModel
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description
        }).AsNoTrackingWithIdentityResolution()
        .ToListAsync() ?? [];
    }

    public async Task<BrandViewModel?> GetAsync(Guid id)
    {
        _logger.LogInformation("Getting the brand with id: {0}", id);
        var brand = await _catalogDbContext.Brands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
        {
            return null;
        }
        return new BrandViewModel
        {
            Id = brand.Id,
            Name = brand.Name,
            Description = brand.Description
        };
    }

    public async Task<BrandViewModel> CreateAsync(BrandInputModel model)
    {
        _logger.LogInformation("Creating a new brand with name: {0}", model.Name);
        Brand brand = new()
        {
            Name = model.Name,
            Description = model.Description
        };

        var entry = await _catalogDbContext.Brands.AddAsync(brand);
        await _catalogDbContext.SaveChangesAsync();

        return new BrandViewModel
        {
            Id = brand.Id,
            Name = brand.Name,
            Description = brand.Description
        };
    }

    public async Task<BrandViewModel?> UpdateAsync(Guid id, BrandInputModel model)
    {
        _logger.LogInformation("Updated the brand: {0}, with id: {1}", model.Name, id);
        var brand = await _catalogDbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
        if (brand == null)
        {
            return null;
        }

        brand.Name = model.Name;
        brand.Description = model.Description;
        var entry = _catalogDbContext.Brands.Update(brand);
        await _catalogDbContext.SaveChangesAsync();

        return new BrandViewModel
        {
            Id = entry.Entity.Id,
            Name = entry.Entity.Name,
            Description = entry.Entity.Description
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting a brand with id: {0}", id);
        var brand = await _catalogDbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
        if (brand == null)
        {
            return false;
        }

        brand.IsDeleted = true;
        _catalogDbContext.Brands.Update(brand);
        await _catalogDbContext.SaveChangesAsync();
        return true;
    }
}
