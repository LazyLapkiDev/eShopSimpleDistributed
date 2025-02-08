using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityUserService;

public static class AuthOptions
{
    public const string ISSUER = "IdentityUserService";
    const string KEY = "mysupersecret_secretsecretsecretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(Encoding.UTF8.GetBytes(KEY));
}
