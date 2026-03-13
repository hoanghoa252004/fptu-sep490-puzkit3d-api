using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways.VNPAY;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways;

internal sealed class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentGatewayFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ResultT<IPaymentGateway> GetGateway(string provider)
    {
        if(string.IsNullOrEmpty(provider))
        {
            return Result.Failure<IPaymentGateway>(
                Domain.Entities.Payments.PaymentError.UnsupportedPaymentProvider(provider));
        }

        return provider.ToUpperInvariant() switch
        {
            "VNPAY" => Result.Success<IPaymentGateway>(
                _serviceProvider.GetRequiredService<VNPAYGateway>()),

            _ => Result.Failure<IPaymentGateway>(
                Domain.Entities.Payments.PaymentError.UnsupportedPaymentProvider(provider))
        };
    }
}

