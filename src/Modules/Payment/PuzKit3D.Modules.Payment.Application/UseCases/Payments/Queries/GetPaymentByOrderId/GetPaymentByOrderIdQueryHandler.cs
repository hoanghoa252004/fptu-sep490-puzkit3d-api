using MediatR;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Payments.Queries.GetPaymentByOrderId;

internal sealed class GetPaymentByOrderIdQueryHandler : IQueryHandler<GetPaymentByOrderIdQuery, GetPaymentByOrderIdResponse>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly ICurrentUser _currentUser;

    public GetPaymentByOrderIdQueryHandler(
        IPaymentRepository paymentRepository,
        IOrderReplicaRepository orderReplicaRepository,
        ICurrentUser currentUser)
    {
        _paymentRepository = paymentRepository;
        _orderReplicaRepository = orderReplicaRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<GetPaymentByOrderIdResponse>> Handle(
        GetPaymentByOrderIdQuery query,
        CancellationToken cancellationToken)
    {
        // Verify order exists
        var order = await _orderReplicaRepository.GetByIdAsync(query.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<GetPaymentByOrderIdResponse>(
                PaymentError.OrderNotFound(query.OrderId));
        }

        // Verify current user is the owner of the order
        if (!Guid.TryParse(_currentUser.UserId, out var userId) || userId != order.CustomerId)
        {
            return Result.Failure<GetPaymentByOrderIdResponse>(
                PaymentError.UnauthorizedAccessToOrder());
        }

        // Get payment by order id
        var payment = await _paymentRepository.GetByOrderIdAsync(query.OrderId, cancellationToken);
        if (payment is null)
        {
            return Result.Failure<GetPaymentByOrderIdResponse>(
                PaymentError.PaymentNotFound(query.OrderId));
        }

        return Result.Success(new GetPaymentByOrderIdResponse(
            PaymentId: payment.Id.Value,
            OrderId: payment.ReferenceOrderId,
            OrderType: payment.ReferenceOrderType,
            Amount: payment.Amount,
            PaymentMethod: payment.PaymentMethod,
            Status: payment.Status.ToString(),
            ExpiredAt: payment.ExpiredAt,
            PaidAt: payment.PaidAt,
            CreatedAt: payment.CreatedAt,
            UpdatedAt: payment.UpdatedAt));
    }
}
