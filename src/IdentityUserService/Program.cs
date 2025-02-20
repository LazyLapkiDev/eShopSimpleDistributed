using CommonConfigurationExtensions;
using IdentityUserService;
using IdentityUserService.IntegrationEvents;
using IdentityUserService.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleRabbitEventBus;
using SimpleRabbitEventBus.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Data;
using UserService.Data.Entities;

namespace UserService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddOptions<AuthOptions>()
            .Bind(builder.Configuration.GetSection("Auth"))
            .Validate(value =>
            {
                if(int.IsPositive(value.ExpirationTimeInDays))
                {
                    return value.ExpirationTimeInDays < 30;
                }
                return false;
            }, "ExpirationTimeInDays must be positive integer less then 30");

        var connectionStr = builder.Configuration.GetConnectionString("PostgreSQL");
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionStr,
                x => x.MigrationsHistoryTable("__MigrationsHistory", typeof(Program).Assembly.GetName().Name));
        });

        builder.Services.AddAuthorization();
        using var rsa = RSA.Create();
        builder.Services.AddCommonAuthentication(builder.Configuration, rsa);

        builder.Services.AddSimpleEventBus(builder.Configuration);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/healthz");

        app.MapGet("/users", async (HttpContext httpContext, AppDbContext dbContext) =>
        {
            return await dbContext.Users.ToListAsync();
        })
        .WithDescription("Return a list of users")
        .WithName("Users")
        .RequireAuthorization();

        app.MapPost("/login", async (LoginInputModel input, HttpContext httpContext, AppDbContext dbContext, IOptions<AuthOptions> authOptions) =>
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == input.Email);
            if(user is null)
            {
                return Results.NotFound();
            }

            byte[] salt = Convert.FromBase64String(user.Salt);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (hashedPassword != user.PasswordHash)
            {
                return Results.Unauthorized();
            }

            var claims = new List<Claim> 
            { 
                new (ClaimTypes.Name, user.Id.ToString())
            };

            var bytes = File.ReadAllBytes(@"D:\DEV_PROJECTS\.NET\C#\2_LearningApps\eShopSimpleDistributed\keys\private_rsa_key.pem");
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(bytes, out _);

            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(rsa),
                SecurityAlgorithms.RsaSha256);

            var jwt = new JwtSecurityToken(
                issuer: authOptions.Value.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(authOptions.Value.ExpirationTimeInDays)),
                signingCredentials: signingCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Results.Ok(new { Token = tokenString });
        })
        .WithDescription("Return a JWT token for user")
        .WithName("Login");

        app.MapPost("/register", async (RegisterInputModel input, 
            HttpContext httpContext, 
            AppDbContext dbContext,
            IEventBus eventBus) =>
        {
            var existingUser = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == input.Email);
            if(existingUser is not null)
            {
                return Results.Conflict("User with this email already exists.");
            }

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = input.UserName,
                Email = input.Email,
                PasswordHash = hashedPassword,
                Salt = Convert.ToBase64String(salt),

                Settings = new()
            };

            var entry = dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();

            var userCreatedEvent = new UserCreatedEvent
            {
                UserId = entry.Entity.Id,
                Email = entry.Entity.Email,
                Culture = entry.Entity.Settings.Culture,
                IsNotificationEnabled = entry.Entity.Settings.IsNotificationEnabled
            };
            await eventBus.PublishAsync(userCreatedEvent);

            return Results.Ok(new { Message = "User registered successfully.", UserId = newUser.Id });
        })
        .WithDescription("Register a new user")
        .WithName("Register");

        app.Run();
    }
}
