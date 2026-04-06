namespace PuzKit3D.Modules.Wallet.Domain.Entities.WalletConfigs;

public class WalletConfig
{
    public Guid Id { get; private set; }
    public decimal OnlineOrderReturnPercentage { get; private set; }
    public decimal OnlineOrderCompletedRewardPercentage { get; private set; }
    public decimal CODOrderCompletedRewardPercentage { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private WalletConfig() { }

    public WalletConfig(
        decimal onlineOrderReturnPercentage,
        decimal onlineOrderCompletedRewardPercentage,
        decimal codOrderCompletedRewardPercentage)
    {
        Id = Guid.NewGuid();
        OnlineOrderReturnPercentage = onlineOrderReturnPercentage;
        OnlineOrderCompletedRewardPercentage = onlineOrderCompletedRewardPercentage;
        CODOrderCompletedRewardPercentage = codOrderCompletedRewardPercentage;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        decimal onlineOrderReturnPercentage,
        decimal onlineOrderCompletedRewardPercentage,
        decimal codOrderCompletedRewardPercentage)
    {
        OnlineOrderReturnPercentage = onlineOrderReturnPercentage;
        OnlineOrderCompletedRewardPercentage = onlineOrderCompletedRewardPercentage;
        CODOrderCompletedRewardPercentage = codOrderCompletedRewardPercentage;
        UpdatedAt = DateTime.UtcNow;
    }
}
