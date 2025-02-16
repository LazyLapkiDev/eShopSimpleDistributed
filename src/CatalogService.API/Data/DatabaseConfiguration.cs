namespace CatalogService.API.Data;

public static class DatabaseConfiguration
{
    public static string SchemaName
    {
        //get => typeof(DatabaseConfiguration).Assembly.GetName().Name ?? throw new ArgumentNullException(nameof(DatabaseConfiguration));
        get => "CatalogService";
    }
}
