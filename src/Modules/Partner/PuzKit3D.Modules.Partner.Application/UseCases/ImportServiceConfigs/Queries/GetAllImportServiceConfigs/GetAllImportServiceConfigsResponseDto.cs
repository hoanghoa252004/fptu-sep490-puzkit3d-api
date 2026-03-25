namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetAllImportServiceConfigs;

public sealed record GetAllImportServiceConfigsResponseDto(
    Guid Id,
    decimal BaseShippingFee,
    string CountryCode,
    string CountryName,
    decimal ImportTaxPercentage,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
