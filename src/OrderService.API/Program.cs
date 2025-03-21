using CommonConfigurationExtensions;
using Microsoft.EntityFrameworkCore;
using OrdersService.API.Data;
using System.Security.Cryptography;
using SimpleRabbitEventBus;
using OrdersService.API.Services;
using OrdersService.API.Api;
using OrdersService.API.Infrastructure.IntegrationEvents.Handlers;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Input;
using OrdersService.API.Infrastructure;
using OrdersService.API.Infrastructure.IntegrationEvents.Events.Output;

namespace OrdersService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionStr = builder.Configuration.GetConnectionString("PostgreSQL");
        builder.Services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseNpgsql(connectionStr, opt => opt.MigrationsHistoryTable("__MigrationsHistory", DatabaseConfiguration.SchemaName));
        });

        builder.Services.AddAuthorization();
        using var rsa = RSA.Create();
        builder.Services.AddCommonAuthentication(builder.Configuration, rsa);

        builder.Services.AddSimpleEventBus(builder.Configuration)
            .AddSubscription<ProductCreatedEvent, ProductCreatedEventHandler>()
            .AddSubscription<ProductUpdatedEvent, ProductUpdatedEventHandler>()
            .AddSubscription<UserVerificatedEvent, UserVerificatedEventHandler>()
            .AddSubscription<OrderCreatedEvent, OrderCreatedEventHandler>()
            .AddSubscription<OrderConfirmationEvent, OrderConfirmationEventHandler>()
            .AddSubscription<OrderRejectEvent, OrderRejectEventHandler>()
            .AddSubscription<ProductsReservedEvent, ProductsReservedEventHandler>();

        builder.Services.AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>();

        builder.Services.AddScoped<OrderSagaOrchestrator>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapOrderEndpoints();

        app.Run();
    }
}
