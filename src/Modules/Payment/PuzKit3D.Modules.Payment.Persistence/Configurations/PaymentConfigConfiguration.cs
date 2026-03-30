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

        builder.Property(pc => pc.OnlinePaymentExpiredInDays)
            .IsRequired()
            .HasColumnName("online_payment_expired_in_days");

        builder.Property(pc => pc.OnlineTransactionExpiredInMinutes)
            .IsRequired()
            .HasColumnName("online_transaction_expired_in_minutes");

        builder.Property(pc => pc.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.ToTable("payment_configs");
    }
}
