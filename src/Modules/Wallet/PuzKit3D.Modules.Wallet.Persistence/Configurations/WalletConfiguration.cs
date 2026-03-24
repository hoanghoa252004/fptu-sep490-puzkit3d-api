using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using WalletEntity = PuzKit3D.Modules.Wallet.Domain.Entities.Wallets.Wallet;
using WalletTransactionEntity = PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions.WalletTransaction;

namespace PuzKit3D.Modules.Wallet.Persistence.Configurations;

internal sealed class WalletConfiguration : IEntityTypeConfiguration<WalletEntity>
{
    public void Configure(EntityTypeBuilder<WalletEntity> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasConversion(
                id => id.Value,
                value => WalletId.From(value));

        builder.Property(w => w.UserId)
            .IsRequired();

        builder.Property(w => w.Balance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();

        builder.HasIndex(w => w.UserId)
            .IsUnique();
    }
}
