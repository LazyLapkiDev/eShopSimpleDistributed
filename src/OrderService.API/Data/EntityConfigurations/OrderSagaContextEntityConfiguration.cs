using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.API.Data.Entities;

namespace OrdersService.API.Data.EntityConfigurations;

public class OrderSagaContextEntityConfiguration : IEntityTypeConfiguration<OrderSagaContext>
{
    public void Configure(EntityTypeBuilder<OrderSagaContext> builder)
    {
        builder.HasKey(x => x.OrderId);

        builder.OwnsMany(x => x.OrderItems, b =>
        {
            b.WithOwner();
            b.ToJson();
        });
    }
}
