namespace IdentityUserService;

public class AuthOptions
{
    public required int ExpirationTimeInDays { get; set; }
    public required string Issuer { get; set; }
    public required string PublicKeyFilePath { get; set; }
}
