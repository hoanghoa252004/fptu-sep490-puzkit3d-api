namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;

public sealed record GetAllAssemblyMethodsPublicResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description);

