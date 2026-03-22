using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Payments.Queries.GetPaymentByOrderId;

public sealed record GetPaymentByOrderIdQuery(Guid OrderId) : IQuery<GetPaymentByOrderIdResponse>;

public sealed record GetPaymentByOrderIdResponse(
Guid PaymentId,
Guid OrderId,
string OrderType,
decimal Amount,
string PaymentMethod,
string Status,
DateTime ExpiredAt,
DateTime? PaidAt,
DateTime CreatedAt,
DateTime UpdatedAt);
