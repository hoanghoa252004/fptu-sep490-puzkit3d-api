using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;

namespace PuzKit3D.Modules.Wallet.Persistence.Configurations;

internal sealed class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.HasKey(wt => wt.Id);

        builder.Property(wt => wt.Id)
            .HasConversion(
                id => id.Value,
                value => WalletTransactionId.From(value));

        builder.Property(wt => wt.UserId)
            .IsRequired();

        builder.Property(wt => wt.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(wt => wt.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(wt => wt.OrderId)
            .IsRequired();

        builder.Property(wt => wt.CreatedAt)
            .IsRequired();

        builder.HasIndex(wt => wt.UserId);
        builder.HasIndex(wt => wt.OrderId);
        builder.HasIndex(wt => wt.Type);
        builder.HasIndex(wt => new { wt.UserId, wt.CreatedAt });
    }
}
