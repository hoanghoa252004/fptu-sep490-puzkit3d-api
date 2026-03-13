using MediatR;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Queries.GetTransactionsByPayment;

internal sealed class GetPaymentTransactionsQueryHandler : IQueryHandler<GetPaymentTransactionsQuery, GetPaymentTransactionsResponse>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly ICurrentUser _currentUser;

    public GetPaymentTransactionsQueryHandler(
        IPaymentRepository paymentRepository,
        ITransactionRepository transactionRepository,
        IOrderReplicaRepository orderReplicaRepository,
        ICurrentUser currentUser)
    {
        _paymentRepository = paymentRepository;
        _transactionRepository = transactionRepository;
        _orderReplicaRepository = orderReplicaRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<GetPaymentTransactionsResponse>> Handle(
        GetPaymentTransactionsQuery query,
        CancellationToken cancellationToken)
    {
        // Verify payment exists
        var payment = await _paymentRepository.GetByIdAsync(PaymentId.From(query.PaymentId), cancellationToken);
        if (payment is null)
        {
            return Result.Failure<GetPaymentTransactionsResponse>(
                PaymentError.PaymentNotFound(query.PaymentId));
        }

        // Verify order exists
        var order = await _orderReplicaRepository.GetByIdAsync(payment.ReferenceOrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<GetPaymentTransactionsResponse>(PaymentError.OrderNotFound(payment.ReferenceOrderId));
        }

        // Verify current user is the owner of the order
        if (!Guid.TryParse(_currentUser.UserId, out var userId) || userId != order.CustomerId)
        {
            return Result.Failure<GetPaymentTransactionsResponse>(PaymentError.UnauthorizedAccessToOrder());
        }

        // Get all transactions for this payment
        var transactions = await _transactionRepository.FindAsync(
            t => t.PaymentId == PaymentId.From(query.PaymentId),
            cancellationToken);

        var transactionDtos = transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto(
                t.Id.Value,
                t.TxnRef,
                t.Provider,
                t.Status.ToString(),
                t.Amount,
                t.PaymentUrl,
                t.ExpiredAt,
                t.TransactionNo,
                t.Status == TransactionStatus.Success ? t.UpdatedAt : null, // PaidAt = UpdatedAt when Success
                t.CreatedAt,
                t.UpdatedAt))
            .ToList();

        return Result.Success(new GetPaymentTransactionsResponse(transactionDtos));
    }
}
