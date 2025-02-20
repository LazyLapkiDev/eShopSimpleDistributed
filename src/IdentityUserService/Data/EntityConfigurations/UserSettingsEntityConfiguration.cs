using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Data.Entities;

namespace IdentityUserService.Data.EntityConfigurations;

public class UserSettingsEntityConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.Property(x => x.Culture)
            .IsRequired();
        builder.HasIndex(x => x.UserId);
    }
}
