using Microsoft.EntityFrameworkCore;
using UserService.Data.Entities;

namespace UserService.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }


    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
