using Microsoft.EntityFrameworkCore;
using OrdersService.API.Data.Entities;

namespace OrdersService.API.Data;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderSagaContext> OrderSagaContexts { get; set; }

    public OrderDbContext(DbContextOptions options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConfiguration.SchemaName);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
