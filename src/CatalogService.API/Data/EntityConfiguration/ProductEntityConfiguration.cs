using CatalogService.API.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.API.Data.EntityConfiguration;

public class ProductEntityConfiguration : BaseEntityTypeConfiguration<Product>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(e => e.BrandId);
        builder.HasIndex(e => e.CategoryId);
    }
}
