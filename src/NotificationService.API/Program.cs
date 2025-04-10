
using CommonConfigurationExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.API.Data;
using NotificationService.API.IntegrationEvents.EventHandling;
using NotificationService.API.IntegrationEvents.Events;
using NotificationService.API.Services;
using SimpleRabbitEventBus;
using System.Security.Cryptography;

namespace NotificationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionStr = builder.Configuration.GetConnectionString("PostgreSQL");
            builder.Services.AddDbContext<NotificationDbContext>(options =>
            {
                options.UseNpgsql(connectionStr, opt => opt.MigrationsHistoryTable("__MigrationsHistory", DatabaseConfiguration.SchemaName));
            });


            builder.Services.AddAuthorization();
            using var rsa = RSA.Create();
            builder.Services.AddCommonAuthentication(builder.Configuration, rsa);

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<INotificationService, NotificationSendingService>();
            builder.Services.AddScoped<UserCreatedEventHandler>();

            builder.Services.AddSimpleEventBus(builder.Configuration)
                .AddSubscription<UserCreatedEvent, UserCreatedEventHandler>();

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
}
