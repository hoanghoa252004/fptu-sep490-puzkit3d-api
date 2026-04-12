using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetAssemblyMethodsByCapabilityAndMaterial;

public sealed record GetAssemblyMethodsByCapabilityAndMaterialQuery(
    Guid CapabilityId,
    Guid MaterialId) : IQuery<List<GetAssemblyMethodBasicResponseDto>>;
