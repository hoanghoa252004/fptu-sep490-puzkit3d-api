using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderConfigs;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockOrderConfigConfiguration : IEntityTypeConfiguration<InstockOrderConfig>
{
    public void Configure(EntityTypeBuilder<InstockOrderConfig> builder)
    {
        builder.HasKey(ic => ic.Id);

        builder.Property(ic => ic.Id)
            .HasColumnName("id");

        builder.Property(ic => ic.OrderMustCompleteInDays)
            .IsRequired()
            .HasColumnName("order_must_complete_in_days");

        builder.Property(ic => ic.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.ToTable("instock_order_configs");
    }
}
