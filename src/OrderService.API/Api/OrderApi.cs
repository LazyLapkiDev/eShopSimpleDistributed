using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrdersService.API.Mapping;
using OrdersService.API.Models;
using OrdersService.API.Services;

namespace OrdersService.API.Api;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/orders");

        api.MapGet("/", GetOrdersAsync);
        api.MapGet("/{id:guid}", GetOrderByIdAsync);
        api.MapPost("/", CreateOrderAsync);

        return routeBuilder;
    }

    [Produces("application/json")]
    public static async Task<Ok<PaginatedResult<OrderViewModel>>> GetOrdersAsync(HttpContext httpContext,
        IOrderService orderService,
        [AsParameters] PaginationRequest paginationRequest)
    {
        var orders = await orderService.GetOrdersAsync(paginationRequest);
        return TypedResults.Ok(new PaginatedResult<OrderViewModel>(orders.PageIndex, 
            orders.PageSize, 
            orders.Count, 
            orders.Data.Select(Mapper.MapToOrderViewModel)));
    }

    [Produces("application/json")]
    public static async Task<Results<Ok<OrderViewModel>, NotFound>> GetOrderByIdAsync(HttpContext httpContext,
        IOrderService orderService,
        Guid id)
    {
        var result = await orderService.GetOrderAsync(id);
        if(!result.Success)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(Mapper.MapToOrderViewModel(result.Data!));
    }

    [Consumes("application/json")]
    [Produces("application/json")]
    public static async Task<Results<Ok<Guid>, BadRequest<string>>> CreateOrderAsync(HttpContext httpContext,
        IOrderService orderService,
        [FromBody]IEnumerable<OrderItemViewModel> orderItemViewModels)
    {
        var success = Guid.TryParse(httpContext.User.Identity?.Name, out var userId);
        if(!success)
        {
            return TypedResults.BadRequest("User information required");
        }
        var order = Mapper.MapToOrder(userId, orderItemViewModels);
        var result = await orderService.CreateOrderAsync(order);

        if(!result.Success)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.Ok(result.Data!.Id);
    }
}
