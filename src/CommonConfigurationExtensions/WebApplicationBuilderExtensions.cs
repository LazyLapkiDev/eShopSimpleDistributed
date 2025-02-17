using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CommonConfigurationExtensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddCommonAuthentication(this IServiceCollection services, string publicKeyFilePath)
    {
        var bytes = File.ReadAllBytes(publicKeyFilePath);
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(bytes, out _);
        services.AddAuthentication(options =>
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
                ValidateIssuerSigningKey = true,
                ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
            };
        });
        return services;
    }
}
