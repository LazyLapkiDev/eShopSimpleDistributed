using IdentityUserService;
using IdentityUserService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        var expirationTime = builder.Configuration.GetValue<double>("Auth:ExpirationTimeInDays");
        var connectionStr = builder.Configuration.GetConnectionString("PostgreSQL");
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionStr,
                x => x.MigrationsHistoryTable("__MigrationsHistory", typeof(Program).Assembly.GetName().Name));
        });

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateLifetime = true,
                ValidateAudience = false,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
        });

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
        app.UseAuthorization();

        app.MapHealthChecks("/healthz");

        app.MapGet("/users", async (HttpContext httpContext, AppDbContext dbContext) =>
        {
            return await dbContext.Users.ToListAsync();
        })
        .WithDescription("Return a list of users")
        .WithName("Users")
        .RequireAuthorization();

        app.MapPost("/login", async (LoginInputModel input, HttpContext httpContext, AppDbContext dbContext) =>
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

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationTime)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Results.Ok(new { Token = tokenString });
        })
        .WithDescription("Return a JWT token for user")
        .WithName("Login");

        app.MapPost("/register", async (RegisterInputModel input, HttpContext httpContext, AppDbContext dbContext) =>
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

            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "User registered successfully.", UserId = newUser.Id });
        })
        .WithDescription("Register a new user")
        .WithName("Register");

        app.Run();
    }
}
