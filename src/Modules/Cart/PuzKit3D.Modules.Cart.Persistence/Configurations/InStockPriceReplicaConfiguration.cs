using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockPriceReplicaConfiguration : IEntityTypeConfiguration<InStockPriceReplica>
{
    public void Configure(EntityTypeBuilder<InStockPriceReplica> builder)
    {
        builder.ToTable("instock_price_replica");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(p => p.EffectiveFrom)
            .IsRequired()
            .HasColumnName("effective_from");

        builder.Property(p => p.EffectiveTo)
            .IsRequired()
            .HasColumnName("effective_to");

        builder.Property(p => p.Priority)
            .IsRequired()
            .HasColumnName("priority");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.Ignore(p => p.DomainEvents);
    }
}
