using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

namespace PuzKit3D.Modules.Payment.Persistence.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TransactionId.From(value));

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.PaymentId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => PaymentId.From(value));

        builder.Property(t => t.Provider)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.PaymentUrl)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(t => t.TransactionNo)
            .HasColumnType("text");

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(t => t.RawResponsePayload)
            .HasColumnType("text");

        builder.Property(t => t.ExpiredAt)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.HasIndex(t => t.Code).IsUnique();
        builder.HasIndex(t => t.PaymentId);
        builder.HasIndex(t => t.Status);
    }
}
