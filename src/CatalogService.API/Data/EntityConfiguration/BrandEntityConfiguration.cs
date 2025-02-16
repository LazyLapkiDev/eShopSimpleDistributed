using CatalogService.API.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.API.Data.EntityConfiguration;

public class BrandEntityConfiguration : BaseEntityTypeConfiguration<Brand>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Brand> builder)
    {
        
    }
}
