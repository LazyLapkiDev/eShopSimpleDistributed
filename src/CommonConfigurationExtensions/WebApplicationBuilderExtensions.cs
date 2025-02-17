using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CommonConfigurationExtensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddCommonAuthentication(this IServiceCollection services,
        RSA rsa,
        string issuer,
        string[] securityAlgorithms)
    {
        if (rsa is null)
        {
            throw new ArgumentNullException(nameof(rsa));
        }

        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentNullException(nameof(issuer));
        }

        if (securityAlgorithms.Length == 0)
        {
            throw new ArgumentException("The array cannot be empty.", nameof(securityAlgorithms));
        }

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
                ValidIssuer = issuer,
                ValidateLifetime = true,
                ValidateAudience = false,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateIssuerSigningKey = true,
                ValidAlgorithms = securityAlgorithms
            };
        });
        return services;
    }
}