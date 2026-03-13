using Microsoft.AspNetCore.Http;

namespace PuzKit3D.Modules.Payment.Application.Abstractions;

public interface IVnPaySignatureValidator
{
    bool ValidateSignature(IQueryCollection queryCollection, string secureHash);
    
    string? GetResponseData(IQueryCollection queryCollection, string key);
}
