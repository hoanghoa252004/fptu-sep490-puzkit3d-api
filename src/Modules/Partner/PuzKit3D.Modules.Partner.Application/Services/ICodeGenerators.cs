namespace PuzKit3D.Modules.Partner.Application.Services;

public interface IPartnerProductRequestCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}

public interface IPartnerProductQuotationCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}

public interface IPartnerProductOrderCodeGenerator
{
    Task<string> GenerateNextCodeAsync(CancellationToken cancellationToken = default);
}
