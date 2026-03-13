using Microsoft.AspNetCore.Http;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Queries.GetTransactionsByPayment;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Commands.CreateTransaction;

internal sealed class CreateTransactionCommandHandler : ICommandTHandler<CreateTransactionCommand, string>
{
    private readonly ICurrentUser _currentUser;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(
        ICurrentUser currentUser,
        IOrderReplicaRepository orderReplicaRepository,
        IPaymentRepository paymentRepository,
        ITransactionRepository transactionRepository,
        IPaymentGatewayFactory paymentGatewayFactory,
        IPaymentUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _orderReplicaRepository = orderReplicaRepository;
        _paymentRepository = paymentRepository;
        _transactionRepository = transactionRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<string>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Verify payment exists
        var payment = await _paymentRepository.GetByIdAsync(PaymentId.From(request.paymentId), cancellationToken);
        if (payment is null)
        {
            return Result.Failure<string>(
                PaymentError.PaymentNotFound(request.paymentId));
        }

        if (payment.Status == PaymentStatus.Paid)
        {
            return Result.Failure<string>(PaymentError.PaymentAlreadyPaid());
        }

        // Verify order exists
        var order = await _orderReplicaRepository.GetByIdAsync(payment.ReferenceOrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<string>(PaymentError.OrderNotFound(payment.ReferenceOrderId));
        }

        // Verify current user is the owner of the order
        if (!Guid.TryParse(_currentUser.UserId, out var userId) || userId != order.CustomerId)
        {
            return Result.Failure<string>(PaymentError.UnauthorizedAccessToOrder());
        }

        
        //if (payment.ExpiredAt< DateTime.UtcNow)
        //{
        //    var updateStatusResult = payment.UpdateStatus(PaymentStatus.Expired);
        //    if (updateStatusResult.IsFailure)
        //    {
        //        return Result.Failure<string>(updateStatusResult.Error);
        //    }
        //    return Result.Failure<string>(PaymentError.PaymentExpired());
        //}

        // Check if there's an active (non-expired) transaction for this payment
        var activeTransactions = await _transactionRepository.FindAsync(
            t => t.PaymentId == payment.Id && t.ExpiredAt > DateTime.UtcNow,
            cancellationToken);

        if (activeTransactions.Any())
        {
            return Result.Failure<string>(PaymentError.ActiveTransactionExists());
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var gatewayResult = _paymentGatewayFactory.GetGateway(request.provider);
            if (gatewayResult.IsFailure)
            {
                return Result.Failure<string>(gatewayResult.Error);
            }

            var paymentGateway = gatewayResult.Value;

            var transactionResult = Transaction.Create(
                paymentId: payment.Id,
                provider: paymentGateway.ProviderName,
                status: TransactionStatus.Pending,
                amount: payment.Amount);

            if (transactionResult.IsFailure)
            {
                return Result.Failure<string>(transactionResult.Error);
            }

            var transaction = transactionResult.Value;
            _transactionRepository.Add(transaction);

            var txnRef = DateTime.Now.Ticks.ToString();

            var paymentUrlParams = new CreatePaymentUrlParams(
                Amount: payment.Amount,
                Description: $"Payment for ORDER[{order.Code}] _ CUSTOMER {order.Id} ",
                TxnRef: txnRef,
                transactionResult.Value.CreatedAt,
                transactionResult.Value.ExpiredAt);

            var paymentUrlResult = paymentGateway.CreatePaymentUrl(request.context, paymentUrlParams);

            if (paymentUrlResult.IsFailure)
            {
                return Result.Failure<string>(paymentUrlResult.Error);
            }

            // Save payment URL to transaction
            transaction.SetPaymentUrl(paymentUrlResult.Value);
            // Save txn ref to transaction for later use in IPN:
            transaction.SetTxnRef(txnRef);

            return Result.Success(paymentUrlResult.Value);
        }, cancellationToken);
    }
}
