using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using VNPAY;
using VNPAY.Models;
using VNPAY.Models.Enums;
using VNPAY.Models.Exceptions;

namespace PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways;

internal sealed class VnpayGateway : IPaymentGateway
{
    private readonly IVnpayClient _vnpayClient;
    private readonly ILogger<VnpayGateway> _logger;

    public string ProviderName => "VNPAY";

    public VnpayGateway(IVnpayClient vnpayClient, ILogger<VnpayGateway> logger)
    {
        _vnpayClient = vnpayClient;
        _logger = logger;
    }

    public ResultT<string> CreatePaymentUrl(CreatePaymentUrlParams @params)
    {
        var vnpayRequest = new VnpayPaymentRequest
        {
            Money = Convert.ToDouble(@params.Amount),
            Description = @params.Description, 
            BankCode = BankCode.ANY,
            Language = DisplayLanguage.Vietnamese
        };

        var paymentUrlInfo = _vnpayClient.CreatePaymentUrl(vnpayRequest);

        return Result.Success(paymentUrlInfo.Url);
        
    }

    public Result ProceedAfterPayment(HttpRequest request)
    {
        var paymentResult = _vnpayClient.GetPaymentResult(request);


    }
}
