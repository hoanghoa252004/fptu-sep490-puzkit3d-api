using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Queries.GetTransactionsByPayment;

public sealed record GetPaymentTransactionsQuery(Guid PaymentId) : IQuery<GetPaymentTransactionsResponse>;

public sealed record GetPaymentTransactionsResponse(List<TransactionDto> Transactions);

public sealed record TransactionDto(
    Guid Id,
    string TxnRef,
    string Provider,
    string Status,
    decimal Amount,
    string? PaymentUrl,
    DateTime ExpiredAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);
