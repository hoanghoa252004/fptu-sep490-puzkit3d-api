using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

public static class TransactionError
{
    public static Error InvalidCode() =>
        Error.Validation("Transaction.InvalidCode", "Transaction code is required and cannot exceed 10 characters.");

    public static Error InvalidPaymentId() =>
        Error.Validation("Transaction.InvalidPaymentId", "Payment ID cannot be empty.");

    public static Error InvalidProvider() =>
        Error.Validation("Transaction.InvalidProvider", "Provider is required and cannot exceed 30 characters.");

    public static Error InvalidTransactionNo(int maxLength) =>
        Error.Validation("Transaction.InvalidTransactionNo", $"Transaction number cannot exceed {maxLength} characters.");

    public static Error InvalidAmount() =>
        Error.Validation("Transaction.InvalidAmount", "Amount must be greater than 0.");

    public static Error InvalidStatus() =>
        Error.Validation("Transaction.InvalidStatus", "Transaction status is invalid.");
}
