using Microsoft.AspNetCore.Http;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Payments;

internal sealed class CreatePaymentUrlCommandHandler : ICommandTHandler<CreatePaymentUrlCommand, string>
{
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public CreatePaymentUrlCommandHandler(
        IOrderReplicaRepository orderReplicaRepository,
        IPaymentRepository paymentRepository,
        ITransactionRepository transactionRepository,
        IPaymentGateway paymentGateway,
        IPaymentUnitOfWork unitOfWork)
    {
        _orderReplicaRepository = orderReplicaRepository;
        _paymentRepository = paymentRepository;
        _transactionRepository = transactionRepository;
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<string>> Handle(CreatePaymentUrlCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<string>(PaymentError.OrderNotFound(request.OrderId));
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

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var transactionCode = $"{order.Code}";

            var transactionResult = Transaction.Create(
                code: transactionCode,
                paymentId: payment.Id,
                provider: _paymentGateway.ProviderName,
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

            var paymentUrlResult = _paymentGateway.CreatePaymentUrl(request.context ,paymentUrlParams);

            if (paymentUrlResult.IsFailure)
            {
                return Result.Failure<string>(paymentUrlResult.Error);
            }

            return Result.Success(paymentUrlResult.Value);
        }, cancellationToken);
    }
}
