using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

namespace PuzKit3D.Modules.Payment.Persistence.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transaction");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TransactionId.From(value))
            .HasColumnName("id");

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("code");

        builder.Property(t => t.PaymentId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => PaymentId.From(value))
            .HasColumnName("payment_id");

        builder.Property(t => t.Provider)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("provider");

        builder.Property(t => t.TransactionNo)
            .HasMaxLength(300)
            .HasColumnName("transaction_no");

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("status");

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasColumnName("amount");

        builder.Property(t => t.RawResponsePayload)
            .HasColumnType("jsonb")
            .HasColumnName("raw_response_payload");

        builder.Property(t => t.ExpiredAt)
            .IsRequired()
            .HasColumnName("expired_at");

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(t => t.Code).IsUnique();
        builder.HasIndex(t => t.PaymentId);
        builder.HasIndex(t => t.Status);
    }
}
