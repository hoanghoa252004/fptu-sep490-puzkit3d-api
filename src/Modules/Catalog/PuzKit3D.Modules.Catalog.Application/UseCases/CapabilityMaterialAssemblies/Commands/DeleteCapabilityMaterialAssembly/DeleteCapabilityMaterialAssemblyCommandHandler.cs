using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.DeleteCapabilityMaterialAssembly;

internal sealed class DeleteCapabilityMaterialAssemblyCommandHandler : ICommandHandler<DeleteCapabilityMaterialAssemblyCommand>
{
    private readonly ICapabilityMaterialAssemblyRepository _repository;
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteCapabilityMaterialAssemblyCommandHandler(
        ICapabilityMaterialAssemblyRepository repository,
        IAssemblyMethodRepository assemblyMethodRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _assemblyMethodRepository = assemblyMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteCapabilityMaterialAssemblyCommand request,
        CancellationToken cancellationToken)
    {
        var assemblyMethodId = AssemblyMethodId.From(request.AssemblyMethodId);
        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);

        if (assemblyMethod is null)
            return Result.Failure(AssemblyMethodError.NotFound(request.AssemblyMethodId));

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var id = CapabilityMaterialAssemblyId.From(request.CapabilityMaterialAssemblyId);
            var item = await _repository.GetByIdAsync(id, cancellationToken);

            if (item is null)
                return Result.Failure(CapabilityMaterialAssemblyError.NotFound(request.CapabilityMaterialAssemblyId));

            if (item.AssemblyId != assemblyMethodId)
                return Result.Failure(CapabilityMaterialAssemblyError.NotFound(request.CapabilityMaterialAssemblyId));

            // Delete the item
            _repository.Delete(item);

            return Result.Success();
        });
    }
}
