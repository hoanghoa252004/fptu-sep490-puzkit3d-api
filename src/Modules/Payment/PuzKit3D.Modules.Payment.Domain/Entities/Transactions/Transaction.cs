using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

public class Transaction : Entity<TransactionId>
{
    public string Code { get; private set; } = null!;
    public PaymentId PaymentId { get; private set; } = null!;
    public string Provider { get; private set; } = null!;
    public string? TransactionNo { get; private set; }
    public TransactionStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public string? RawResponsePayload { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Payments.Payment Payment { get; private set; } = null!;

    private Transaction(
        TransactionId id,
        string code,
        PaymentId paymentId,
        string provider,
        TransactionStatus status,
        decimal amount,
        string? transactionNo,
        string? rawResponsePayload,
        DateTime createdAt) : base(id)
    {
        Code = code;
        PaymentId = paymentId;
        Provider = provider;
        Status = status;
        Amount = amount;
        TransactionNo = transactionNo;
        RawResponsePayload = rawResponsePayload;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Transaction() : base()
    {
    }

    public static ResultT<Transaction> Create(
        string code,
        PaymentId paymentId,
        string provider,
        TransactionStatus status,
        decimal amount,
        string? transactionNo = null,
        string? rawResponsePayload = null,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length > 10)
            return Result.Failure<Transaction>(TransactionError.InvalidCode());

        if (paymentId is null)
            return Result.Failure<Transaction>(TransactionError.InvalidPaymentId());

        if (string.IsNullOrWhiteSpace(provider) || provider.Length > 30)
            return Result.Failure<Transaction>(TransactionError.InvalidProvider());

        if (transactionNo?.Length > 300)
            return Result.Failure<Transaction>(TransactionError.InvalidTransactionNo(300));

        if (amount <= 0)
            return Result.Failure<Transaction>(TransactionError.InvalidAmount());

        if (!Enum.IsDefined(typeof(TransactionStatus), status))
            return Result.Failure<Transaction>(TransactionError.InvalidStatus());

        var transaction = new Transaction(
            TransactionId.Create(),
            code,
            paymentId,
            provider,
            status,
            amount,
            transactionNo,
            rawResponsePayload,
            createdAt ?? DateTime.UtcNow
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
            if (transactionNo.Length > 300)
                return Result.Failure(TransactionError.InvalidTransactionNo(300));
            TransactionNo = transactionNo;
        }
        if (rawResponsePayload != null)
        {
            RawResponsePayload = rawResponsePayload;
        }
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
