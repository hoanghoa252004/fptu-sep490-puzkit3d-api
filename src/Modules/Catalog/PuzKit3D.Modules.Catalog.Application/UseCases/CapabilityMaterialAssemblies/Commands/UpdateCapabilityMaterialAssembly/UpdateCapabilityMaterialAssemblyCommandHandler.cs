using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.UpdateCapabilityMaterialAssembly;

internal sealed class UpdateCapabilityMaterialAssemblyCommandHandler
    : ICommandTHandler<UpdateCapabilityMaterialAssemblyCommand, Guid>
{
    private readonly ICapabilityMaterialAssemblyRepository _repository;
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateCapabilityMaterialAssemblyCommandHandler(
        ICapabilityMaterialAssemblyRepository repository,
        IAssemblyMethodRepository assemblyMethodRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _assemblyMethodRepository = assemblyMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdateCapabilityMaterialAssemblyCommand request,
        CancellationToken cancellationToken)
    {
        var assemblyMethodId = AssemblyMethodId.From(request.AssemblyMethodId);
        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);

        if (assemblyMethod is null)
            return Result.Failure<Guid>(AssemblyMethodError.NotFound(request.AssemblyMethodId));

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var id = CapabilityMaterialAssemblyId.From(request.CapabilityMaterialAssemblyId);
            var item = await _repository.GetByIdAsync(id, cancellationToken);

            if (item is null)
                return Result.Failure<Guid>(CapabilityMaterialAssemblyError.NotFound(request.CapabilityMaterialAssemblyId));

            if (item.AssemblyId != assemblyMethodId)
                return Result.Failure<Guid>(CapabilityMaterialAssemblyError.NotFound(request.CapabilityMaterialAssemblyId));

            // Update the item
            item.SetActive(request.IsActive);

            // Update in repository
            _repository.Update(item);

            return Result.Success(item.Id.Value);
        }, cancellationToken);
    }
}
