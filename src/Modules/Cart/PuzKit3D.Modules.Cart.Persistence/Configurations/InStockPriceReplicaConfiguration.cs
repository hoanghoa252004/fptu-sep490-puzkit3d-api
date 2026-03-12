using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockPriceReplicaConfiguration : IEntityTypeConfiguration<InStockPriceReplica>
{
    public void Configure(EntityTypeBuilder<InStockPriceReplica> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.EffectiveFrom)
            .IsRequired();

        builder.Property(p => p.EffectiveTo)
            .IsRequired();

        builder.Property(p => p.Priority)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.Ignore(p => p.DomainEvents);
    }
}
