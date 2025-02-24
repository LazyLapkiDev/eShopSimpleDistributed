using CatalogService.API.Data;
using CatalogService.API.Data.Entities;
using CatalogService.API.IntegrationEvents.Events;
using CatalogService.API.Models;
using CatalogService.API.Models.Brand;
using CatalogService.API.Models.Category;
using CatalogService.API.Models.Product;
using CatalogService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SimpleRabbitEventBus.Abstractions;

namespace CatalogService.API.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IEventBus _eventBus;

    public ProductService(ILogger<ProductService> logger,
        CatalogDbContext catalogDbContext,
        IEventBus eventBus)
    {
        _logger = logger;
        _catalogDbContext = catalogDbContext;
        _eventBus = eventBus;
    }

    public async Task<PaginatedResult<ProductListViewModel>> GetPaginatedAsync(PaginationRequest paginationRequest,
        string? category,
        string? brand,
        string? name)
    {
        _logger.LogInformation("Getting a list of products");
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var query = _catalogDbContext.Products
            .Include(p => p.Brand)
            .AsQueryable()
            .AsNoTrackingWithIdentityResolution();

        var categoryResult = Guid.TryParse(category, out var categoryId);
        if (categoryResult)
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }

        var brandResult = Guid.TryParse(brand, out var brandId);
        if (categoryResult)
        {
            query = query.Where(x => x.BrandId == brandId);
        }

        if (name is not null)
        {
            query = query.Where(x => x.Name.StartsWith(name));
        }

        var totalItems = await query
            .LongCountAsync();

        var itemsOnPage = await query
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Brand = new BrandViewModel
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                }
            })
            .ToListAsync();

        return new PaginatedResult<ProductListViewModel>(pageIndex, pageSize, totalItems, itemsOnPage);
    }

    public async Task<ProductViewModel?> GetAsync(Guid id)
    {
        _logger.LogInformation("Getting the product with id: {0}", id);
        var product = await _catalogDbContext.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            _logger.LogInformation("The product with id: {0} not found", id);
            return null;
        }

        return new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Brand = new BrandViewModel
            {
                Id = product.Brand.Id,
                Name = product.Brand.Name
            },
            Description = product.Description,
            Category = new CategoryViewModel
            {
                Id = product.Category.Id,
                Name = product.Category.Name
            },
            AvailableQuantity = product.StockQuantity - product.ReservedQuantity,
            Price = product.Price
        };
    }

    public async Task<Guid> CreateAsync(ProductInputModel model)
    {
        _logger.LogInformation("Create a new product with name: {0}", model.Name);
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            CategoryId = model.CategoryId,
            BrandId = model.BrandId,
            StockQuantity = model.StockQuantity,
            ReservedQuantity = 0
        };

        var entry = await _catalogDbContext.Products.AddAsync(product);
        try
        {
            
            await _catalogDbContext.SaveChangesAsync();
            var createProductEvent = new CreateProductEvent
            { 
                ProductId = entry.Entity.Id,
                Name = entry.Entity.Name,
                Price = entry.Entity.Price
            };
            await _eventBus.PublishAsync(createProductEvent);
            
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Create product fail, name {0}, error: {1}", model.Name, ex.Message);
            throw;
        }
        return entry.Entity.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, ProductInputModel model)
    {
        _logger.LogInformation("Update a product with id: {0}", id);
        var product = await _catalogDbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return false;
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        product.BrandId = model.BrandId;

        var entry = _catalogDbContext.Products.Update(product);
        await _catalogDbContext.SaveChangesAsync();
        try
        {

            await _catalogDbContext.SaveChangesAsync();
            var updateProductEvent = new UpdateProductEvent(
                entry.Entity.Id,
                entry.Entity.Price,
                entry.Entity.Name
            );
            await _eventBus.PublishAsync(updateProductEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update product fail, id {0}, error: {1}", id, ex.Message);
            throw;
        }
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting a product with id: {0}", id);
        var product = await _catalogDbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return false;
        }

        _catalogDbContext.Products.Remove(product);
        await _catalogDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStockAsync(Guid id, int stock, bool isIncrease)
    {
        _logger.LogInformation("Increase a stock of product with id: {0}, by {1}", id, stock);
        var product = await _catalogDbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return false;
        }

        product.StockQuantity = isIncrease ? product.StockQuantity + stock : product.StockQuantity - stock;
        _catalogDbContext.Products.Update(product);
        await _catalogDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReserveAsync(Guid id, int count)
    {
        var product = await _catalogDbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return false;
        }

        product.ReservedQuantity += count;
        _catalogDbContext.Products.Update(product);
        await _catalogDbContext.SaveChangesAsync();
        return true;
    }
}
