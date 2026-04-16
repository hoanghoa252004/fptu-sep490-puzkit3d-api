using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Payments;

public class Payment : AggregateRoot<PaymentId>
{
    private readonly List<Transaction> _transactions = new();
    
    public Guid ReferenceOrderId { get; private set; }
    public string ReferenceOrderType { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    private Payment(
        PaymentId id,
        Guid referenceOrderId,
        string referenceOrderType,
        decimal amount,
        string paymentMethod,
        PaymentStatus status,
        DateTime expiredAt,
        DateTime createdAt) : base(id)
    {
        ReferenceOrderId = referenceOrderId;
        ReferenceOrderType = referenceOrderType;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = status;
        ExpiredAt = expiredAt;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Payment() : base()
    {
    }

    public static ResultT<Payment> Create(
        Guid referenceOrderId,
        string referenceOrderType,
        decimal amount,
        string paymentMethod,
        int expirationDays = 1)
    {
        if (referenceOrderId == Guid.Empty)
            return Result.Failure<Payment>(PaymentError.InvalidReferenceOrderId());

        if (string.IsNullOrWhiteSpace(referenceOrderType) || referenceOrderType.Length > 30)
            return Result.Failure<Payment>(PaymentError.InvalidReferenceOrderType());

        if (amount <= 0)
            return Result.Failure<Payment>(PaymentError.InvalidAmount());

        if (string.IsNullOrWhiteSpace(paymentMethod))
            return Result.Failure<Payment>(PaymentError.InvalidPaymentMethod());

        var now = DateTime.UtcNow;
        var expiredAt = paymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase)
            ? now.AddMinutes(expirationDays)
            : now.AddMonths(1);

        var payment = new Payment(
            PaymentId.Create(),
            referenceOrderId,
            referenceOrderType,
            amount,
            paymentMethod,
            PaymentStatus.Pending,
            expiredAt,
            now
        );

        return Result.Success(payment);
    }

    public Result UpdateStatus(PaymentStatus status, DateTime? paidAt = null)
    {
        if (!Enum.IsDefined(typeof(PaymentStatus), status))
            return Result.Failure(PaymentError.InvalidStatus());

        Status = status;

        if (paidAt.HasValue)
        {
            PaidAt = paidAt;
        }

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result AddTransaction(Transaction transaction)
    {
        if (transaction.PaymentId != Id)
            return Result.Failure(Error.Validation("Payment.InvalidTransaction", "Transaction does not belong to this payment."));

        _transactions.Add(transaction);
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
