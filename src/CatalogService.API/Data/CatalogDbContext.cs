using CatalogService.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API.Data;

public class CatalogDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }

    public CatalogDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.SchemaName);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
        foreach (var entry in entries) 
        { 
            var currentDate = DateTime.UtcNow;
            var currentEntity = entry.Entity as BaseEntity;
            if(currentEntity != null)
            {
                if (entry.State == EntityState.Added)
                {
                    currentEntity.CreatedAt = currentDate;
                    currentEntity.UpdatedAt = currentDate;
                }
                if (entry.State == EntityState.Modified)
                {
                    currentEntity.UpdatedAt = currentDate;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
