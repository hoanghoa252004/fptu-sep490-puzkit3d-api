using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.DeleteAssemblyMethod;

internal sealed class DeleteAssemblyMethodCommandHandler : ICommandHandler<DeleteAssemblyMethodCommand>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;
    private readonly ICapabilityMaterialAssemblyRepository _capabilityMaterialAssemblyRepository;

    public DeleteAssemblyMethodCommandHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICatalogUnitOfWork unitOfWork,
        ICapabilityMaterialAssemblyRepository capabilityMaterialAssemblyRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _unitOfWork = unitOfWork;
        _capabilityMaterialAssemblyRepository = capabilityMaterialAssemblyRepository;
    }

    public async Task<Result> Handle(DeleteAssemblyMethodCommand request, CancellationToken cancellationToken)
    {
        // Get assembly method by ID
        var assemblyMethodId = AssemblyMethodId.From(request.Id);
        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);

        if (assemblyMethod is null)
        {
            return Result.Failure(AssemblyMethodError.NotFound(request.Id));
        }

        // Check if assembly method is associated with any capability material assemblies
        var existingAssociations = await _capabilityMaterialAssemblyRepository
            .GetCapabilityMaterialAssembliesByAssemblyMethodIdAsync(assemblyMethodId, cancellationToken);

        if (existingAssociations is not null && existingAssociations.Any())
        {
            return Result.Failure(AssemblyMethodError.InUse(request.Id));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            assemblyMethod.Delete();
            // Delete from repository
            _assemblyMethodRepository.Delete(assemblyMethod);

            return Result.Success();
        }, cancellationToken);
    }
}

