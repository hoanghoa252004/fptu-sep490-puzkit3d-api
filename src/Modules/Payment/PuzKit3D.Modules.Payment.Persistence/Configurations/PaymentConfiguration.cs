using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;

namespace PuzKit3D.Modules.Payment.Persistence.Configurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Domain.Entities.Payments.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Payments.Payment> builder)
    {
        builder.ToTable("payment");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PaymentId.From(value))
            .HasColumnName("id");

        builder.Property(p => p.ReferenceOrderId)
            .IsRequired()
            .HasColumnName("reference_order_id");

        builder.Property(p => p.ReferenceOrderType)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("reference_order_type");

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasColumnName("amount");

        builder.Property(p => p.Provider)
            .HasMaxLength(30)
            .HasColumnName("provider");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("status");

        builder.Property(p => p.ExpiredAt)
            .HasColumnName("expired_at");

        builder.Property(p => p.PaidAt)
            .HasColumnName("paid_at");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasMany(p => p.Transactions)
            .WithOne(t => t.Payment)
            .HasForeignKey(t => t.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Transactions).AutoInclude(false);

        builder.HasIndex(p => p.ReferenceOrderId);
        builder.HasIndex(p => p.Status);
    }
}
