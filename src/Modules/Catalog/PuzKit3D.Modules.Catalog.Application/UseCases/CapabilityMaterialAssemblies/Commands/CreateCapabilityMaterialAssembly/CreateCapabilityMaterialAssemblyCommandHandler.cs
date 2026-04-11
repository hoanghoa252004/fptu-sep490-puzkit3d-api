using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.CreateCapabilityMaterialAssembly;

internal sealed class CreateCapabilityMaterialAssemblyCommandHandler : ICommandTHandler<CreateCapabilityMaterialAssemblyCommand, object>
{
    private readonly ICapabilityMaterialAssemblyRepository _repository;
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateCapabilityMaterialAssemblyCommandHandler(
        ICapabilityMaterialAssemblyRepository repository,
        IAssemblyMethodRepository assemblyMethodRepository,
        ICapabilityRepository capabilityRepository,
        IMaterialRepository materialRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _assemblyMethodRepository = assemblyMethodRepository;
        _capabilityRepository = capabilityRepository;
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<object>> Handle(
        CreateCapabilityMaterialAssemblyCommand request,
        CancellationToken cancellationToken)
    {
        var assemblyMethodId = AssemblyMethodId.From(request.AssemblyMethodId);
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var materialId = MaterialId.From(request.MaterialId);

        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);
        if (assemblyMethod is null)
        {
            return Result.Failure<object>(AssemblyMethodError.NotFound(request.AssemblyMethodId));
        }

        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);
        if (capability is null)
        {
            return Result.Failure<object>(CapabilityError.NotFound(request.CapabilityId));
        }

        var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);
        if (material is null)
        {
            return Result.Failure<object>(MaterialError.NotFound(request.MaterialId));
        }

        var existing = await _repository.FindAsync(
            cma => cma.AssemblyId == assemblyMethodId && 
                   cma.CapabilityId == capabilityId && 
                   cma.MaterialId == materialId,
            cancellationToken);

        if (existing.Any())
        {
            return Result.Failure<object>(
                Error.Conflict(
                    "CapabilityMaterialAssembly.AlreadyExists",
                    "A capability material assembly with this combination already exists."));
        }

        var result = CapabilityMaterialAssembly.Create(capabilityId, materialId, assemblyMethodId, request.IsActive);
        if (result.IsFailure)
            return Result.Failure<object>(result.Error);

        var cma = result.Value;
        _repository.Add(cma);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<object>(cma.Id.Value);
    }
}
