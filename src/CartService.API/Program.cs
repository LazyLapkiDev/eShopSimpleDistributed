
using CartService.API.Api;
using CartService.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using CommonConfigurationExtensions;
using CartService.API.Services;

namespace CartService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        using var rsa = RSA.Create();
        builder.Services.AddCommonAuthentication(builder.Configuration, rsa);

        var connectionStr = builder.Configuration.GetConnectionString("PostgreSQL");
        builder.Services.AddDbContext<CartDbContext>(options =>
        {
            options.UseNpgsql(connectionStr, opt => opt.MigrationsHistoryTable("__MigrationsHistory", DatabaseConfiguration.SchemaName));
        });

        builder.Services.AddScoped<CartManagerService>();

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

        app.MapCartApi();

        app.Run();
    }
}
