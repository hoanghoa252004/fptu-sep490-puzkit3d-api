namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetAssemblyMethodsByCapabilityAndMaterial;

public sealed record GetAssemblyMethodBasicResponseDto(
    Guid Id,
    string Name,
    string Slug);
