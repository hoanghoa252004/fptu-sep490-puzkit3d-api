using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetCapabilityMaterialAssembliesByAssemblyMethodId;

internal sealed class QueryHandler : IQueryHandler<GetCapabilityMaterialAssembliesByAssemblyMethodIdQuery, object>
{
    private readonly ICapabilityMaterialAssemblyRepository _repository;
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ICurrentUser _currentUser;

    public QueryHandler(
        ICapabilityMaterialAssemblyRepository repository,
        IAssemblyMethodRepository assemblyMethodRepository,
        ICapabilityRepository capabilityRepository,
        IMaterialRepository materialRepository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _assemblyMethodRepository = assemblyMethodRepository;
        _capabilityRepository = capabilityRepository;
        _materialRepository = materialRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetCapabilityMaterialAssembliesByAssemblyMethodIdQuery request,
        CancellationToken cancellationToken)
    {
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        var assemblyMethodId = AssemblyMethodId.From(request.AssemblyMethodId);
        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);

        if (assemblyMethod is null)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFound(request.AssemblyMethodId));
        }

        if (!isStaffOrManager && !assemblyMethod.IsActive)
        {
            return Result.Failure<object>(
                Error.Unauthorized(
                    "AssemblyMethod.NoPermissionGranted",
                    "You do not have permission to view this assembly method."));
        }

        var items = await _repository.FindAsync(
            cma => cma.AssemblyId == assemblyMethodId,
            cancellationToken);

        var itemList = items.ToList();

        var capabilityIds = itemList.Select(x => x.CapabilityId).Distinct().ToList();
        var materialIds = itemList.Select(x => x.MaterialId).Distinct().ToList();

        var capabilities = await _capabilityRepository.FindAsync(
            c => capabilityIds.Contains(c.Id),
            cancellationToken);

        var materials = await _materialRepository.FindAsync(
            m => materialIds.Contains(m.Id),
            cancellationToken);

        var capabilityDict = capabilities.ToDictionary(x => x.Id);
        var materialDict = materials.ToDictionary(x => x.Id);

        var responses = new List<object>();

        foreach (var cma in itemList)
        {
            if (!isStaffOrManager && !cma.IsActive)
                continue;

            if (!capabilityDict.TryGetValue(cma.CapabilityId, out var capability) ||
                !materialDict.TryGetValue(cma.MaterialId, out var material))
                continue;

            if (isStaffOrManager)
            {
                responses.Add(new GetCapabilityMaterialAssemblyDetailedResponseDto(
                    cma.Id.Value,
                    new(
                        capability.Id.Value,
                        capability.Name),
                    new(
                        material.Id.Value,
                        material.Name),
                    cma.AssemblyId.Value,
                    cma.IsActive));
            }
            else
            {
                responses.Add(new GetCapabilityMaterialAssemblyResponseDto(
                    cma.Id.Value,
                    new(
                        capability.Id.Value,
                        capability.Name),
                    new(
                        material.Id.Value,
                        material.Name),
                    cma.AssemblyId.Value));
            }
        }

        return Result.Success<object>(responses);
    }
}
