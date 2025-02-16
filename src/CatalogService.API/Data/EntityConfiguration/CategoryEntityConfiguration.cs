using CatalogService.API.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.API.Data.EntityConfiguration;

public class CategoryEntityConfiguration : BaseEntityTypeConfiguration<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
    {

    }
}
