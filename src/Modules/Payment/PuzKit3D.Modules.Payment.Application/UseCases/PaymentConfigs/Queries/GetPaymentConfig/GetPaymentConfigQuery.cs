using MediatR;
using PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Queries.GetPaymentConfig;

public record GetPaymentConfigQuery : IQuery<GetPaymentConfigResponse>;

public record GetPaymentConfigResponse(
    Guid Id,
    int OnlinePaymentExpiredValue,
    string OnlinePaymentExpiredUnit,
    int OnlineTransactionExpiredValue,
    string OnlineTransactionExpiredUnit,
    DateTime UpdatedAt);

