namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;

public sealed record GetAllAssemblyMethodsResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

