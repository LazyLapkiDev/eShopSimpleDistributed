using CatalogService.API.Api;
using CatalogService.API.Data;
using CatalogService.API.Services;
using CatalogService.API.Services.Interfaces;
using CommonConfigurationExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using SimpleRabbitEventBus;
using CatalogService.API.IntegrationEvents.Handlers;
using CatalogService.API.IntegrationEvents.Events;

namespace CatalogService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization(options  =>
            {
                //options.AddPolicy("admin", policy =>
                //{
                //    policy.RequireClaim(ClaimTypes.Role, ["admin"]);
                //});
            });
            using var rsa = RSA.Create();
            builder.Services.AddCommonAuthentication(builder.Configuration, rsa);

            var connectionStr = builder.Configuration.GetConnectionString("PostgreSQL");
            builder.Services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseNpgsql(connectionStr, x => x.MigrationsHistoryTable("__MigrationsHistory", DatabaseConfiguration.SchemaName));
            });

            builder.Services.AddHealthChecks();

            builder.Services.AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IBrandService, BrandService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IReservationService, ReservationService>();

            builder.Services.AddSimpleEventBus(builder.Configuration)
                .AddSubscription<OrderItemsReserveEvent, OrderItemsReserveEventHandler>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapHealthChecks("/heathz");

            app.MapCatalogApi();

            app.Run();
        }
    }
}
