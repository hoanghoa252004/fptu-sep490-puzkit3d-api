namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigById;

public sealed record GetImportServiceConfigByIdResponseDto(
    Guid Id,
    decimal BaseShippingFee,
    string CountryCode,
    string CountryName,
    decimal ImportTaxPercentage,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
