using Microsoft.AspNetCore.Http;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Application.Services;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Payments;

internal sealed class CreatePaymentUrlCommandHandler : ICommandTHandler<CreatePaymentUrlCommand, string>
{
    private readonly ICurrentUser _currentUser;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory;
    private readonly ITransactionCodeGenerator _transactionCodeGenerator;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public CreatePaymentUrlCommandHandler(
        ICurrentUser currentUser,
        IOrderReplicaRepository orderReplicaRepository,
        IPaymentRepository paymentRepository,
        ITransactionRepository transactionRepository,
        IPaymentGatewayFactory paymentGatewayFactory,
        ITransactionCodeGenerator transactionCodeGenerator,
        IPaymentUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _orderReplicaRepository = orderReplicaRepository;
        _paymentRepository = paymentRepository;
        _transactionRepository = transactionRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
        _transactionCodeGenerator = transactionCodeGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<string>> Handle(CreatePaymentUrlCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<string>(PaymentError.OrderNotFound(request.OrderId));
        }

        if (!Guid.TryParse(_currentUser.UserId, out var userId) || userId != order.CustomerId)
        {
            return Result.Failure<string>(PaymentError.UnauthorizedAccessToOrder());
        }

        var payment = await _paymentRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);

        if (payment is null)
        {
            return Result.Failure<string>(PaymentError.PaymentNotFound(request.OrderId));
        }

        if (payment.Status == PaymentStatus.Paid)
        {
            return Result.Failure<string>(PaymentError.PaymentAlreadyPaid());
        }

        if (payment.ExpiredAt< DateTime.UtcNow)
        {
            var updateStatusResult = payment.UpdateStatus(PaymentStatus.Expired);
            if (updateStatusResult.IsFailure)
            {
                return Result.Failure<string>(updateStatusResult.Error);
            }
            return Result.Failure<string>(PaymentError.PaymentExpired());
        }

        // Check if there's an active (non-expired) transaction for this payment
        var activeTransaction = await _transactionRepository.FindAsync(
            t => t.PaymentId == payment.Id && t.ExpiredAt > DateTime.UtcNow,
            cancellationToken);

        if (activeTransaction.Any())
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
            var transactionCode = await _transactionCodeGenerator.GenerateNextCodeAsync(cancellationToken);

            var transactionResult = Transaction.Create(
                code: transactionCode,
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

            var paymentUrlParams = new CreatePaymentUrlParams(
                Amount: payment.Amount,
                Description: $"Payment for ORDER[{order.Code}] _ CUSTOMER {order.Id} ",
                transactionResult.Value.CreatedAt,
                transactionResult.Value.ExpiredAt);

            var paymentUrlResult = paymentGateway.CreatePaymentUrl(request.context, paymentUrlParams);

            if (paymentUrlResult.IsFailure)
            {
                return Result.Failure<string>(paymentUrlResult.Error);
            }

            return Result.Success(paymentUrlResult.Value);
        }, cancellationToken);
    }
}
