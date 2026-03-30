using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Queries.GetPaymentConfig;

public record GetPaymentConfigQuery : IQuery<GetPaymentConfigResponse>;

public record GetPaymentConfigResponse(
    Guid Id,
    int OnlinePaymentExpiredInDays,
    int OnlineTransactionExpiredInMinutes,
    DateTime UpdatedAt);
