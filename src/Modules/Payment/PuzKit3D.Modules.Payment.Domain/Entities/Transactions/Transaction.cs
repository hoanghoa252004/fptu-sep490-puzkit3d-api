using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

public class Transaction : Entity<TransactionId>
{
    public string TxnRef { get; private set; } = null!;
    public PaymentId PaymentId { get; private set; } = null!;
    public string Provider { get; private set; } = null!;
    public string? TransactionNo { get; private set; }
    public string? PaymentUrl { get; private set; }
    public TransactionStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public string? RawResponsePayload { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Payments.Payment Payment { get; private set; } = null!;

    private Transaction(
        TransactionId id,
        PaymentId paymentId,
        string provider,
        TransactionStatus status,
        decimal amount,
        DateTime expiredAt,
        DateTime createdAt) : base(id)
    {
        PaymentId = paymentId;
        Provider = provider;
        Status = status;
        Amount = amount;
        ExpiredAt = expiredAt;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Transaction() : base()
    {
    }

    public static ResultT<Transaction> Create(
        PaymentId paymentId,
        string provider,
        TransactionStatus status,
        decimal amount)
    {
        if (paymentId is null)
            return Result.Failure<Transaction>(TransactionError.InvalidPaymentId());

        if (string.IsNullOrWhiteSpace(provider) || provider.Length > 30)
            return Result.Failure<Transaction>(TransactionError.InvalidProvider());

        if (amount <= 0)
            return Result.Failure<Transaction>(TransactionError.InvalidAmount());

        if (!Enum.IsDefined(typeof(TransactionStatus), status))
            return Result.Failure<Transaction>(TransactionError.InvalidStatus());

        var transaction = new Transaction(
            TransactionId.Create(),
            paymentId,
            provider,
            status,
            amount,
            DateTime.UtcNow.AddMinutes(5),
            DateTime.UtcNow
        );

        return Result.Success(transaction);
    }

    public Result UpdateStatus(TransactionStatus status, string? transactionNo = null, string? rawResponsePayload = null)
    {
        if (!Enum.IsDefined(typeof(TransactionStatus), status))
            return Result.Failure(TransactionError.InvalidStatus());

        Status = status;
        if (!string.IsNullOrWhiteSpace(transactionNo))
        {
            TransactionNo = transactionNo;
        }
        if (rawResponsePayload != null)
        {
            RawResponsePayload = rawResponsePayload;
        }
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result UpdateToSuccess(string? transactionNo = null, string? rawResponsePayload = null)
    {
        return UpdateStatus(TransactionStatus.Success, transactionNo, rawResponsePayload);
    }

    public Result UpdateToFailed(string? rawResponsePayload = null)
    {
        return UpdateStatus(TransactionStatus.Failed, null, rawResponsePayload);
    }

    public void SetPaymentUrl(string paymentUrl)
    {
        if (!string.IsNullOrWhiteSpace(paymentUrl))
        {
            PaymentUrl = paymentUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void SetTxnRef(string txnRef)
    {
        if (!string.IsNullOrWhiteSpace(txnRef))
        {
            TxnRef = txnRef;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

