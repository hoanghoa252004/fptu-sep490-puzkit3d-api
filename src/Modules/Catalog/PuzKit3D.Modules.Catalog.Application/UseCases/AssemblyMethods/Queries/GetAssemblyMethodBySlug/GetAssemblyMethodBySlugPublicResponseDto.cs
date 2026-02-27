namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

public sealed record GetAssemblyMethodBySlugPublicResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description);

