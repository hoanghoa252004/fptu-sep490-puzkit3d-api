namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigsForSelect;

public sealed record GetImportServiceConfigsForSelectResponseDto(
    Guid Id,
    string CountryName,
    string CountryCode);
