using IdentityUserService.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using IdentityUserService.Data.Entities;

namespace IdentityUserService.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(typeof(Program).Assembly.GetName().Name);
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserSettingsEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
