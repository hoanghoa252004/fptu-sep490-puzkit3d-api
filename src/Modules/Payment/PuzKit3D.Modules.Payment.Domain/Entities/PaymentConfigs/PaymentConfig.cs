namespace PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;

public class PaymentConfig
{
    public Guid Id { get; private set; }
    public int OnlinePaymentExpiredValue { get; private set; }
    public TimeUnit OnlinePaymentExpiredUnit { get; private set; }
    public int OnlineTransactionExpiredValue { get; private set; }
    public TimeUnit OnlineTransactionExpiredUnit { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PaymentConfig() { }

    public PaymentConfig(
        int onlinePaymentExpiredValue,
        TimeUnit onlinePaymentExpiredUnit,
        int onlineTransactionExpiredValue,
        TimeUnit onlineTransactionExpiredUnit)
    {
        Id = Guid.NewGuid();
        OnlinePaymentExpiredValue = onlinePaymentExpiredValue;
        OnlinePaymentExpiredUnit = onlinePaymentExpiredUnit;
        OnlineTransactionExpiredValue = onlineTransactionExpiredValue;
        OnlineTransactionExpiredUnit = onlineTransactionExpiredUnit;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        int onlinePaymentExpiredValue,
        TimeUnit onlinePaymentExpiredUnit,
        int onlineTransactionExpiredValue,
        TimeUnit onlineTransactionExpiredUnit)
    {
        OnlinePaymentExpiredValue = onlinePaymentExpiredValue;
        OnlinePaymentExpiredUnit = onlinePaymentExpiredUnit;
        OnlineTransactionExpiredValue = onlineTransactionExpiredValue;
        OnlineTransactionExpiredUnit = onlineTransactionExpiredUnit;
        UpdatedAt = DateTime.UtcNow;
    }
}
