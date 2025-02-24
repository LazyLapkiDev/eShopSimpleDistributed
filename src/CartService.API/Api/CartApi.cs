using CartService.API.Models;
using CartService.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.API.Api;

public static class CartApi
{
    public static IEndpointRouteBuilder MapCartApi(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/cart")
            .WithTags("Cart");

        api.MapGet("/", GetCartAsync)
            .WithName("GetCartByUser")
            .RequireAuthorization();

        api.MapPost("/", UpdateCartAsync)
            .WithName("UpdateCart")
            .RequireAuthorization();

        api.MapDelete("/", CleanCartAsync)
            .WithName("DeleteCart")
            .RequireAuthorization();

        return routeBuilder;
    }

    public static async Task<Results<Ok<CartViewModel>, BadRequest>> GetCartAsync(HttpContext httpContext,
        CartManagerService cartManager)
    {
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var success = Guid.TryParse(userIdClaim?.Value, out var userId);
        if (!success)
        {
            return TypedResults.BadRequest();
        }
        var cart = await cartManager.GetAsync(userId);
        return TypedResults.Ok(cart);
    }

    public static async Task<Results<NoContent, BadRequest>> UpdateCartAsync(HttpContext httpContext,
        CartManagerService cartManager,
        [FromBody]CartItemInputModel[] input)
    {
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var success = Guid.TryParse(userIdClaim?.Value, out var userId);
        if (!success)
        {
            return TypedResults.BadRequest();
        }
        await cartManager.UpdateAsync(userId, input);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, BadRequest>> CleanCartAsync(HttpContext httpContext,
        CartManagerService cartManager)
    {
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var success = Guid.TryParse(userIdClaim?.Value, out var userId);
        if (!success)
        {
            return TypedResults.BadRequest();
        }
        await cartManager.CleanAsync(userId);
        return TypedResults.NoContent();
    }
}
