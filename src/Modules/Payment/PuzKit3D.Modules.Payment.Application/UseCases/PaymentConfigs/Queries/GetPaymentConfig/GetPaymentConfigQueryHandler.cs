using MediatR;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Queries.GetPaymentConfig;

internal sealed class GetPaymentConfigQueryHandler : IQueryHandler<GetPaymentConfigQuery,GetPaymentConfigResponse>
{
    private readonly IPaymentConfigRepository _paymentConfigRepository;

    public GetPaymentConfigQueryHandler(IPaymentConfigRepository paymentConfigRepository)
    {
        _paymentConfigRepository = paymentConfigRepository;
    }

    public async Task<ResultT<GetPaymentConfigResponse>> Handle(
        GetPaymentConfigQuery request,
        CancellationToken cancellationToken)
    {
        var paymentConfig = await _paymentConfigRepository.GetFirstAsync(cancellationToken);

        if (paymentConfig is null)
        {
            return Result.Failure<GetPaymentConfigResponse>(
                PaymentError.PaymentConfigNotFound());
        }

        var response = new GetPaymentConfigResponse(
            paymentConfig.Id,
            paymentConfig.OnlinePaymentExpiredValue,
            paymentConfig.OnlinePaymentExpiredUnit.ToString(),
            paymentConfig.OnlineTransactionExpiredValue,
            paymentConfig.OnlineTransactionExpiredUnit.ToString(),
            paymentConfig.UpdatedAt);

        return Result.Success(response);
    }
}
