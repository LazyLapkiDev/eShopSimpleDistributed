using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CommonConfigurationExtensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddCommonAuthentication(this IServiceCollection services,
        IConfiguration configuration,
        string[]? securityAlgorithms = null)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        var issuer = configuration.GetValue<string>("Auth:Issuer");
        var keyFilePath = configuration.GetValue<string>("Auth:PublicKeyFilePath");

        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentNullException(nameof(issuer));
        }

        if (string.IsNullOrEmpty(keyFilePath))
        {
            throw new ArgumentNullException(nameof(keyFilePath));
        }


        securityAlgorithms ??= [SecurityAlgorithms.RsaSha256];

        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(File.ReadAllBytes(keyFilePath!), out _);
        services.AddSingleton(rsa);

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