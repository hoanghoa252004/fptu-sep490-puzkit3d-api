using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;

namespace PuzKit3D.Modules.Payment.Persistence.Configurations;

internal sealed class PaymentConfigConfiguration : IEntityTypeConfiguration<PaymentConfig>
{
    public void Configure(EntityTypeBuilder<PaymentConfig> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Id)
            .HasColumnName("id");

        builder.Property(pc => pc.OnlinePaymentExpiredValue)
            .IsRequired()
            .HasColumnName("online_payment_expired_value");

        builder.Property(pc => pc.OnlinePaymentExpiredUnit)
            .IsRequired()
            .HasColumnName("online_payment_expired_unit")
            .HasConversion(
                v => v.ToString(),
                v => (TimeUnit)Enum.Parse(typeof(TimeUnit), v));

        builder.Property(pc => pc.OnlineTransactionExpiredValue)
            .IsRequired()
            .HasColumnName("online_transaction_expired_value");

        builder.Property(pc => pc.OnlineTransactionExpiredUnit)
            .IsRequired()
            .HasColumnName("online_transaction_expired_unit")
            .HasConversion(
                v => v.ToString(),
                v => (TimeUnit)Enum.Parse(typeof(TimeUnit), v));

        builder.Property(pc => pc.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.ToTable("payment_configs");
    }
}

