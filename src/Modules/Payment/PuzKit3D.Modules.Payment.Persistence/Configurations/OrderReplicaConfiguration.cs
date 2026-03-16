using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;

namespace PuzKit3D.Modules.Payment.Persistence.Configurations;

internal sealed class OrderReplicaConfiguration : IEntityTypeConfiguration<OrderReplica>
{
    public void Configure(EntityTypeBuilder<OrderReplica> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IsPaid)
            .IsRequired();

        builder.Property(x => x.PaidAt);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.Code);
        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.Type);
    }
}
