namespace PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;

public class PaymentConfig
{
    public Guid Id { get; private set; }
    public int OnlinePaymentExpiredInDays { get; private set; }
    public int OnlineTransactionExpiredInMinutes { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PaymentConfig() { }

    public PaymentConfig(int onlinePaymentExpiredInDays, int onlineTransactionExpiredInMinutes)
    {
        Id = Guid.NewGuid();
        OnlinePaymentExpiredInDays = onlinePaymentExpiredInDays;
        OnlineTransactionExpiredInMinutes = onlineTransactionExpiredInMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(int onlinePaymentExpiredInDays, int onlineTransactionExpiredInMinutes)
    {
        OnlinePaymentExpiredInDays = onlinePaymentExpiredInDays;
        OnlineTransactionExpiredInMinutes = onlineTransactionExpiredInMinutes;
        UpdatedAt = DateTime.UtcNow;
    }
}
