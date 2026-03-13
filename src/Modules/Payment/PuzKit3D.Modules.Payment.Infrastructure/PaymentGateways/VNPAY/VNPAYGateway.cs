using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways.VNPAY;

internal sealed class VNPAYGateway : IPaymentGateway
{
    private readonly IConfiguration _configuration;

    public string ProviderName => "VNPAY";

    public VNPAYGateway(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ResultT<string> CreatePaymentUrl(HttpContext context, CreatePaymentUrlParams @params)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]!);
        var createdAt = TimeZoneInfo.ConvertTimeFromUtc(@params.createdAt, timeZoneById);
        var expiredAt = TimeZoneInfo.ConvertTimeFromUtc(@params.expiredAt, timeZoneById);
        var pay = new VnPayLibrary();
        var amount = ((long)@params.Amount * 100).ToString();

        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Command", _configuration["VNPAY:Command"]!);
        pay.AddRequestData("vnp_Version", _configuration["VNPAY:Version"]!);
        pay.AddRequestData("vnp_TmnCode", _configuration["VNPAY:TmnCode"]!);
        pay.AddRequestData("vnp_Locale", _configuration["VNPAY:Locale"]!);
        pay.AddRequestData("vnp_CurrCode", _configuration["VNPAY:CurrCode"]!);
        pay.AddRequestData("vnp_ReturnUrl", _configuration["VNPAY:ReturnUrl"]!);
        pay.AddRequestData("vnp_OrderType", _configuration["VNPAY:OrderType"]!);
        pay.AddRequestData("vnp_BankCode", _configuration["VNPAY:BankCode"]!); 
        pay.AddRequestData("vnp_CreateDate", createdAt.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_ExpireDate", expiredAt.ToString("yyyyMMddHHmmss"));

        pay.AddRequestData("vnp_Amount", amount);
        pay.AddRequestData("vnp_OrderInfo", @params.Description);
        pay.AddRequestData("vnp_TxnRef", @params.TxnRef);

        var baseUrl = _configuration["VNPAY:BaseUrl"]!;
        var hashSecret = _configuration["VNPAY:HashSecret"]!;
        var paymentUrl =
            pay.CreateRequestUrl(baseUrl, hashSecret);

        return Result.Success(paymentUrl);
        
    }

    //public Result ProceedAfterPayment(HttpRequest request)
    //{
    //    var paymentResult = _vnpayClient.GetPaymentResult(request);


    //}
}
