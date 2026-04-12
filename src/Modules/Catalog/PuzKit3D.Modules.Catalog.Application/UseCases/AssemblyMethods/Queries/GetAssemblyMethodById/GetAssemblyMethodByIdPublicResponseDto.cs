namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodById;

public sealed record GetAssemblyMethodByIdPublicResponseDto (
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    decimal FactorPercentage);


