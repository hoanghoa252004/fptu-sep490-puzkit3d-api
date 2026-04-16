using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetAssemblyMethodsByCapabilityAndMaterial;

public sealed record GetAssemblyMethodsByCapabilityAndMaterialQuery(
    List<Guid> CapabilityIds,
    Guid MaterialId) : IQuery<List<GetAssemblyMethodBasicResponseDto>>;
