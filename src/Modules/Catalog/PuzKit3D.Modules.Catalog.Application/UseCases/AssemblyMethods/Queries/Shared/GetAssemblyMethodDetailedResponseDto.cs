namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;

public sealed record GetAssemblyMethodDetailedResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
