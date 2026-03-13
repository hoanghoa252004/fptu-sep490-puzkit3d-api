using MediatR;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Commands.ProcessVnPayIPN;

internal sealed class ProcessVnPayIPNCommandHandler : IRequestHandler<ProcessVnPayIPNCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IPaymentUnitOfWork _unitOfWork;
    private readonly IVnPaySignatureValidator _signatureValidator;
    private readonly ILogger<ProcessVnPayIPNCommandHandler> _logger;

    public ProcessVnPayIPNCommandHandler(
        ITransactionRepository transactionRepository,
        IPaymentRepository paymentRepository,
        IOrderReplicaRepository orderReplicaRepository,
        IPaymentUnitOfWork unitOfWork,
        IVnPaySignatureValidator signatureValidator,
        ILogger<ProcessVnPayIPNCommandHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _paymentRepository = paymentRepository;
        _orderReplicaRepository = orderReplicaRepository;
        _unitOfWork = unitOfWork;
        _signatureValidator = signatureValidator;
        _logger = logger;
    }

    public async Task<Result> Handle(ProcessVnPayIPNCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            try
            {
                var queryCollection = request.QueryParameters;

                // Check if query parameters exist
                if (queryCollection.Count == 0)
                {
                    return Result.Failure(Error.Failure("99", "Input data required"));
                }

                // Extract and validate signature
                var vnp_SecureHash = queryCollection["vnp_SecureHash"].ToString();
                if (string.IsNullOrEmpty(vnp_SecureHash))
                {
                    return Result.Failure(Error.Failure("99", "Secure hash is required"));
                }

                // Validate signature
                var checkSignature = _signatureValidator.ValidateSignature(queryCollection, vnp_SecureHash);
                if (!checkSignature)
                {
                    _logger.LogWarning("Invalid VNPAY signature from query parameters");
                    return Result.Failure(Error.Failure("97", "Invalid signature"));
                }

                // Parse response data
                var txnRefStr = _signatureValidator.GetResponseData(queryCollection, "vnp_TxnRef");
                var transactionNoStr = _signatureValidator.GetResponseData(queryCollection, "vnp_TransactionNo");
                var amountStr = _signatureValidator.GetResponseData(queryCollection, "vnp_Amount");
                var vnp_ResponseCode = _signatureValidator.GetResponseData(queryCollection, "vnp_ResponseCode");
                var vnp_TransactionStatus = _signatureValidator.GetResponseData(queryCollection, "vnp_TransactionStatus");

                if (!long.TryParse(txnRefStr, out var transactionRef) ||
                    !long.TryParse(transactionNoStr, out var vnpayTranId) ||
                    !long.TryParse(amountStr, out var vnp_AmountLong))
                {
                    _logger.LogWarning("Invalid VNPAY response data format");
                    return Result.Failure(Error.Failure("99", "Invalid response data"));
                }

                long vnp_Amount = vnp_AmountLong / 100;

                // Find transaction by Code (transactionRef)
                var transactions = await _transactionRepository.FindAsyncTracking(
                    t => t.TxnRef == transactionRef.ToString(),
                    cancellationToken);
                var transaction = transactions.FirstOrDefault();

                if (transaction == null)
                {
                    _logger.LogWarning("Transaction not found. TxnRef: {TxnRef}", transactionRef);
                    return Result.Failure(Error.Failure("01", "Order not found"));
                }

                // Check if transaction already confirmed
                if (transaction.Status != TransactionStatus.Pending)
                {
                    _logger.LogInformation("Transaction already confirmed. TxnRef: {TxnRef}, Status: {Status}",
                        transactionRef, transaction.Status);
                    return Result.Failure(Error.Failure("02", "Order already confirmed"));
                }

                // Validate amount
                if (transaction.Amount != vnp_Amount)
                {
                    _logger.LogWarning("Amount mismatch. Expected: {Expected}, Received: {Received}",
                        transaction.Amount, vnp_Amount);
                    return Result.Failure(Error.Failure("04", "Invalid amount"));
                }

                // Retrieve the payment
                var payment = await _paymentRepository.GetByIdAsync(transaction.PaymentId, cancellationToken);
                if (payment == null)
                {
                    _logger.LogError("Payment not found for transaction. PaymentId: {PaymentId}", transaction.PaymentId);
                    return Result.Failure(Error.Failure("99", "Payment not found"));
                }

                // Update transaction based on VNPAY response
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    // Payment successful
                    var updateResult = transaction.UpdateToSuccess(vnpayTranId.ToString());
                    if (updateResult.IsFailure)
                    {
                        _logger.LogError("Failed to update transaction to success. TxnRef: {TxnRef}", transactionRef);
                        return Result.Failure(Error.Failure("99", "Failed to update transaction"));
                    }

                    // Update payment status to Paid
                    var paymentUpdateResult = payment.UpdateStatus(PaymentStatus.Paid, DateTime.UtcNow);
                    if (paymentUpdateResult.IsFailure)
                    {
                        _logger.LogError("Failed to update payment status. PaymentId: {PaymentId}", payment.Id);
                        return Result.Failure(Error.Failure("99", "Failed to update payment status"));
                    }

                    // Find and update OrderReplica
                    var orderReplica = await _orderReplicaRepository.GetByIdAsync(payment.ReferenceOrderId, cancellationToken);
                    if (orderReplica != null)
                    {
                        var paidAt = DateTime.UtcNow;
                        orderReplica.MarkAsPaid(paidAt);
                        // Domain event (OrderReplicaPaidDomainEvent) is raised by MarkAsPaid()
                        // DbContext will dispatch it automatically via MediatR publisher

                        _logger.LogInformation("Marked OrderReplica as paid. OrderId: {OrderId}, OrderType: {OrderType}, PaidAt: {PaidAt}",
                            orderReplica.Id, orderReplica.Type, paidAt);
                    }

                    _logger.LogInformation("Payment successful. TxnRef: {TxnRef}, VNPAY TranId: {VNPAYTranId}, PaymentId: {PaymentId}",
                        transactionRef, vnpayTranId, payment.Id);
                }
                else
                {
                    // Payment failed
                    var updateResult = transaction.UpdateToFailed();
                    if (updateResult.IsFailure)
                    {
                        _logger.LogError("Failed to update transaction to failed. TxnRef: {TxnRef}", transactionRef);
                        return Result.Failure(Error.Failure("99", "Failed to update transaction"));
                    }

                    // Update payment status to Failed
                    var paymentUpdateResult = payment.UpdateStatus(PaymentStatus.Failed);
                    if (paymentUpdateResult.IsFailure)
                    {
                        _logger.LogError("Failed to update payment status. PaymentId: {PaymentId}", payment.Id);
                        return Result.Failure(Error.Failure("99", "Failed to update payment status"));
                    }

                    _logger.LogInformation("Payment failed. TxnRef: {TxnRef}, ResponseCode: {ResponseCode}, PaymentId: {PaymentId}",
                        transactionRef, vnp_ResponseCode, payment.Id);
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing VNPAY IPN callback");
                return Result.Failure(Error.Failure("99", "Internal error"));
            }
        });
    }
}
