using CommonConfigurationExtensions;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Data;
using System.Security.Cryptography;
using SimpleRabbitEventBus;

namespace OrderService.API;

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

        builder.Services.AddSimpleEventBus(builder.Configuration);

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

        app.Run();
    }
}
