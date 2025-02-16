using CatalogService.API.Models.Category;
using CatalogService.API.Models.Brand;
using CatalogService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CatalogService.API.Models.Product;
using CatalogService.API.Services;
using CatalogService.API.Models;
using System.ComponentModel;

namespace CatalogService.API.Api;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/catalog");

        api.MapGet("/hello", (HttpContext httpContext) =>
        {
            return Results.Ok("hello");
        })
        .WithName("Hello");

        api.MapGet("/helloWithAuth", (HttpContext httpContext) =>
        {
            return Results.Ok("hello");
        })
        .WithName("HelloAuth")
        .RequireAuthorization();

        #region Categories endpoints
        var categoryGroup = api.MapGroup("/categories")
            .WithTags("Category");
        categoryGroup.MapGet("/", GetAllCategories)
            .WithName("GetCategories")
            .WithDescription("Return list of categories");
        categoryGroup.MapGet("/{id:guid}", GetCategoryAsync)
            .WithName("GetCategory")
            .WithDescription("Return category with list of products");
        categoryGroup.MapDelete("/{id:guid}", DeleteCategoryAsync)
            .WithName("DeleteCategory")
            .RequireAuthorization();
        categoryGroup.MapPost("/", CreateCategoryAsync)
            .WithName("CreateCategory")
            .RequireAuthorization();
        categoryGroup.MapPut("/{id:guid}", UpdateCategoryAsync)
            .WithName("UpdateCategroy")
            .RequireAuthorization();
        #endregion

        #region Brands endpoints
        var brandGroup = api.MapGroup("/brands")
            .WithTags("Brand");
        brandGroup.MapGet("/", GetAllBrandAsync)
            .WithName("GetBrands");

        brandGroup.MapGet("/{id:guid}", GetBrandAsync)
            .WithName("GetBrand");

        brandGroup.MapPost("/", CreateBrandAsync)
            .WithName("CreateBrand");

        brandGroup.MapPut("/{id:guid}", UpdateBrandAsync)
            .WithName("UpdateBrand");

        brandGroup.MapDelete("/{id:guid}", DeleteBrandAsync)
            .WithName("DeleteBrand");
        #endregion

        #region Products endpoints
        var productGroup = api.MapGroup("/products")
            .WithTags("Product");
        productGroup.MapGet("/", GetListOfProductsAsync)
            .WithName("GetProducts")
            .WithDescription("Return a list of products with pagination");
        productGroup.MapGet("/{id:guid}", GetProductAsync)
            .WithName("GetProduct")
            .WithDescription("Return a product by id");
        productGroup.MapPost("/", CreateProductAsync)
            .WithName("CreateProduct")
            .WithDescription("Add a new product to catalog");
        productGroup.MapPut("/{id:guid}", UpdateProductAsync)
            .WithName("UpdateProduct");
        productGroup.MapDelete("/{id:guid}", DeleteProductAsync)
            .WithName("DeleteProduct");
        productGroup.MapPatch("/{id:guid}", UpdateProductStockAsync)
            .WithName("UpdateProductStock")
            .WithDescription("Update quantity of products in stock");
        #endregion

        return routeBuilder;
    }

    public static async Task<Ok<List<CategoryListViewModel>>> GetAllCategories(
        ICategoryService categoryService)
    {
        var categories = await categoryService.GetAllCategoriesAsync();
        return TypedResults.Ok(categories);
    }

    public static async Task<Results<Ok<CategoryViewModel>, NotFound<string>>> GetCategoryAsync(
        [FromRoute] Guid id,
        ICategoryService categoryService)
    {
        var category = await categoryService.GetCategoryAsync(id);
        if(category is null)
        {
            return TypedResults.NotFound("Category not found");
        }
        return TypedResults.Ok(category);
    }

    public static async Task<Results<NoContent, NotFound>> DeleteCategoryAsync(
        [FromRoute] Guid id,
        ICategoryService categoryService)
    {
        var result = await categoryService.DeleteAsync(id);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    public static async Task<Results<Ok<CategoryViewModel>, NotFound>> UpdateCategoryAsync(
        [FromRoute] Guid id,
        [FromBody] CategoryInputModel inputModel,
        ICategoryService categoryService)
    {
        var category = await categoryService.UpdateAsync(id, inputModel);
        return category is null ? TypedResults.NotFound() : TypedResults.Ok(category);
    }

    public static async Task<Ok<CategoryViewModel>> CreateCategoryAsync(
        [FromBody] CategoryInputModel inputModel,
        ICategoryService categoryService)
    {
        CategoryViewModel category = await categoryService.CreateAsync(inputModel);
        return TypedResults.Ok(category);
    }

    public static async Task<Ok<BrandViewModel>> CreateBrandAsync(
        [FromBody] BrandInputModel inputModel,
        IBrandService brandService)
    {
        var brand = await brandService.CreateAsync(inputModel);
        return TypedResults.Ok(brand);
    }

    public static async Task<Results<Ok<BrandViewModel>, NotFound>> UpdateBrandAsync(
        [FromRoute] Guid id,
        [FromBody] BrandInputModel inputModel,
        IBrandService brandService)
    {
        var brand = await brandService.UpdateAsync(id, inputModel);
        if(brand is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(brand);
    }

    public static async Task<Results<NoContent, NotFound>> DeleteBrandAsync(
        [FromRoute] Guid id,
        IBrandService brandService)
    {
        var result = await brandService.DeleteAsync(id);
        if (result == false)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<BrandViewModel>, NotFound>> GetBrandAsync(
        [FromRoute] Guid id,
        IBrandService brandService)
    {
        var brand = await brandService.GetAsync(id);
        if (brand is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(brand);
    }

    public static async Task<Ok<List<BrandViewModel>>> GetAllBrandAsync(
        IBrandService brandService)
    {
        var brands = await brandService.GetAsync();
        return TypedResults.Ok(brands);
    }

    public static async Task<Results<Ok<PaginatedResult<ProductListViewModel>>, BadRequest<ProblemDetails>>> GetListOfProductsAsync(
        [AsParameters]PaginationRequest paginationRequest,
        [Description("The name of the product to return")] string? name,
        [Description("The category of products to return")] string? category,
        [Description("The brand of products to return")] string? brand,
        IProductService productService)
    {
        if (paginationRequest.PageSize <= 0)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid page size",
                Detail = "Page size must be greater than zero",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var products = await productService.GetPaginatedAsync(paginationRequest, category, brand, name);
        return TypedResults.Ok(products);
    }

    public static async Task<Results<Ok<ProductViewModel>, NotFound>> GetProductAsync(
        [FromRoute]Guid id,
        IProductService productService)
    {
        var product = await productService.GetAsync(id);
        if(product is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(product);
    }

    public static async Task<Results<Ok<Guid>, BadRequest<ProblemDetails>>> CreateProductAsync(
        ProductInputModel inputModel,
        IProductService productService)
    {
        if(inputModel.Price < 0)
        {
            return TypedResults.BadRequest<ProblemDetails>(new()
            {
                Detail = "Item id must be provided in the request body."
            });
        }
        var id = await productService.CreateAsync(inputModel);
        return TypedResults.Ok(id);
    }

    public static async Task<Results<Ok<ProductViewModel>, NotFound>> UpdateProductAsync(
        [FromRoute]Guid id,
        ProductInputModel inputModel,
        IProductService productService)
    {
        var product = await productService.GetAsync(id);
        if (product is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(product);
    }

    public static async Task<Results<NoContent, NotFound>> DeleteProductAsync(
        [FromRoute]Guid id,
        IProductService productService)
    {
        var result = await productService.DeleteAsync(id);

        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> UpdateProductStockAsync([FromRoute] Guid id,
        [FromBody]ProductStockInputModel inputModel,
        IProductService productService)
    {
        var result = await productService.UpdateStockAsync(id, inputModel.Count, inputModel.IsIncrease);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
