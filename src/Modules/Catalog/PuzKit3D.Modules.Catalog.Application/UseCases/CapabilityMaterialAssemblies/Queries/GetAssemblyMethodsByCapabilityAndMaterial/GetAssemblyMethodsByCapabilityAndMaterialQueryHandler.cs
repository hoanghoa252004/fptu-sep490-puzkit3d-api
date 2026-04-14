using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetAssemblyMethodsByCapabilityAndMaterial;

internal sealed class GetAssemblyMethodsByCapabilityAndMaterialQueryHandler
    : IQueryHandler<GetAssemblyMethodsByCapabilityAndMaterialQuery, List<GetAssemblyMethodBasicResponseDto>>
{
    private readonly ICapabilityMaterialAssemblyRepository _capabilityMaterialAssemblyRepository;
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAssemblyMethodsByCapabilityAndMaterialQueryHandler(
        ICapabilityMaterialAssemblyRepository capabilityMaterialAssemblyRepository,
        IAssemblyMethodRepository assemblyMethodRepository)
    {
        _capabilityMaterialAssemblyRepository = capabilityMaterialAssemblyRepository;
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public async Task<ResultT<List<GetAssemblyMethodBasicResponseDto>>> Handle(
        GetAssemblyMethodsByCapabilityAndMaterialQuery request,
        CancellationToken cancellationToken)
    {
        if (!request.CapabilityIds.Any())
            return Result.Success(new List<GetAssemblyMethodBasicResponseDto>());

        // Get all active CapabilityMaterialAssemblies for each capability and material combination
        var capabilityIds = request.CapabilityIds
            .Select(id => CapabilityId.From(id))
            .ToList();

        var materialId = MaterialId.From(request.MaterialId);

        var capabilityMaterialAssemblies = await _capabilityMaterialAssemblyRepository.FindAsync(
            cma => capabilityIds.Contains(cma.CapabilityId)
                && cma.MaterialId == materialId
                && cma.IsActive,
            cancellationToken);

        if (!capabilityMaterialAssemblies.Any())
            return Result.Success(new List<GetAssemblyMethodBasicResponseDto>());

        // Get all assembly method IDs from the results
        var assemblyMethodIds = capabilityMaterialAssemblies
            .Select(cma => cma.AssemblyId)
            .Distinct()
            .ToList();

        // Fetch all assembly methods with batch query
        var assemblyMethods = await _assemblyMethodRepository.FindAsync(
            am => assemblyMethodIds.Contains(am.Id) && am.IsActive,
            cancellationToken);

        // Map to DTOs and distinct by name
        var assemblyMethodDtos = assemblyMethods
            .Select(am => new GetAssemblyMethodBasicResponseDto(
                am.Id.Value,
                am.Name,
                am.Slug))
            .DistinctBy(dto => dto.Name)
            .ToList();

        return Result.Success(assemblyMethodDtos);
    }
}
