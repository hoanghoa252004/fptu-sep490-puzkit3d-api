using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Orders;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => OrderId.From(value))
            .HasColumnName("id");

        builder.Property(o => o.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.OwnsOne(o => o.TotalMoney, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("total_money")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        // ? Config navigation property ?? EF Core bi?t private field
        builder.Navigation(o => o.OrderItems)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
