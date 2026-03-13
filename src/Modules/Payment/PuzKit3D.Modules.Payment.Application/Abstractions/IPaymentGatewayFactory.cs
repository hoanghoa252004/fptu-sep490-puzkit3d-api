using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.Abstractions;

public interface IPaymentGatewayFactory
{
    /// <summary>
    /// Gets a payment gateway by provider name
    /// </summary>
    /// <param name="providerName">The provider name (e.g., "VNPAY")</param>
    /// <returns>Result containing the payment gateway or an error if provider is not supported</returns>
    ResultT<IPaymentGateway> GetGateway(string providerName);
}
