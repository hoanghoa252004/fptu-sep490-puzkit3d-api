using Microsoft.AspNetCore.Http;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.Abstractions;

public interface IPaymentGateway
{
    string ProviderName { get; }
    
    ResultT<string> CreatePaymentUrl( HttpContext context, CreatePaymentUrlParams @params);
    
    //Result ProceedAfterPayment(HttpRequest request);
}

public record CreatePaymentUrlParams(
    decimal Amount,
    string Description,
    string TxnRef,
    DateTime createdAt,
    DateTime expiredAt);
