using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.Modules.Catalog.Domain.UnitOfWork;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.DeleteAssemblyMethod;

internal sealed class DeleteAssemblyMethodCommandHandler : ICommandHandler<DeleteAssemblyMethodCommand>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteAssemblyMethodCommandHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAssemblyMethodCommand request, CancellationToken cancellationToken)
    {
        // Get assembly method by ID
        var assemblyMethodId = AssemblyMethodId.From(request.Id);
        var assemblyMethod = _assemblyMethodRepository.FindById(assemblyMethodId);

        if (assemblyMethod is null)
        {
            return Result.Failure(AssemblyMethodError.NotFound(request.Id));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Delete from repository
            _assemblyMethodRepository.Delete(assemblyMethod);

            return Result.Success();
        }, cancellationToken);
    }
}
