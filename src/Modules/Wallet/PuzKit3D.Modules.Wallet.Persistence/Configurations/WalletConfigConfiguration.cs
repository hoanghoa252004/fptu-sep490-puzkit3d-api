using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletConfigs;

namespace PuzKit3D.Modules.Wallet.Persistence.Configurations;

internal sealed class WalletConfigConfiguration : IEntityTypeConfiguration<WalletConfig>
{
    public void Configure(EntityTypeBuilder<WalletConfig> builder)
    {
        builder.HasKey(wc => wc.Id);

        builder.Property(wc => wc.Id)
            .HasColumnName("id");

        builder.Property(wc => wc.OnlineOrderReturnPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)")
            .HasColumnName("online_order_return_percentage");

        builder.Property(wc => wc.OnlineOrderCompletedRewardPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)")
            .HasColumnName("online_order_completed_reward_percentage");

        builder.Property(wc => wc.CODOrderCompletedRewardPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)")
            .HasColumnName("cod_order_completed_reward_percentage");

        builder.Property(wc => wc.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.ToTable("wallet_configs");
    }
}
