namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.Shared;

public sealed record GetCapabilityMaterialAssemblyResponseDto(
    Guid Id,
    CapabilityBasicDto Capability,
    MaterialBasicDto Material,
    Guid AssemblyMethodId);

public sealed record GetCapabilityMaterialAssemblyDetailedResponseDto(
    Guid Id,
    CapabilityBasicDto Capability,
    MaterialBasicDto Material,
    Guid AssemblyMethodId,
    bool IsActive);

public sealed record CapabilityBasicDto(
    Guid Id,
    string Name);

public sealed record MaterialBasicDto(
    Guid Id,
    string Name);
