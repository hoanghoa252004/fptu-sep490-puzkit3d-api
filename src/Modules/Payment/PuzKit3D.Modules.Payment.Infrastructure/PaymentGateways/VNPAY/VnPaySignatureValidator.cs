using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PuzKit3D.Modules.Payment.Application.Abstractions;

namespace PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways.VNPAY;

internal sealed class VnPaySignatureValidator : IVnPaySignatureValidator
{
    private readonly IConfiguration _configuration;

    public VnPaySignatureValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool ValidateSignature(IQueryCollection queryCollection, string secureHash)
    {
        try
        {
            var vnpay = new VnPayLibrary();

            // Add all vnp_* parameters to the validator
            foreach (var (key, value) in queryCollection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            // Get hash secret from configuration
            var hashSecret = _configuration["VNPAY:HashSecret"];
            if (string.IsNullOrEmpty(hashSecret))
            {
                return false;
            }

            // Validate the signature
            return vnpay.ValidateSignature(secureHash, hashSecret);
        }
        catch
        {
            return false;
        }
    }

    public string? GetResponseData(IQueryCollection queryCollection, string key)
    {
        if (queryCollection.TryGetValue(key, out var value))
        {
            return value.ToString();
        }

        return null;
    }
}
