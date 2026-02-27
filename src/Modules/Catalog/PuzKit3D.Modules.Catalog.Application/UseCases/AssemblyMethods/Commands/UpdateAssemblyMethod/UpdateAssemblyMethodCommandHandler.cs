using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.Modules.Catalog.Domain.UnitOfWork;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.UpdateAssemblyMethod;

internal sealed class UpdateAssemblyMethodCommandHandler : ICommandHandler<UpdateAssemblyMethodCommand>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateAssemblyMethodCommandHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAssemblyMethodCommand request, CancellationToken cancellationToken)
    {
        // Get assembly method by ID
        var assemblyMethodId = AssemblyMethodId.From(request.Id);
        var assemblyMethod = _assemblyMethodRepository.FindById(assemblyMethodId);

        if (assemblyMethod is null)
        {
            return Result.Failure(AssemblyMethodError.NotFound(request.Id));
        }

        // Check if slug is being changed to an existing one
        if (assemblyMethod.Slug != request.Slug)
        {
            var existingAssemblyMethod = await _assemblyMethodRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingAssemblyMethod is not null)
            {
                return Result.Failure(AssemblyMethodError.DuplicateSlug(request.Slug));
            }
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Update assembly method
            var updateResult = assemblyMethod.Update(
                request.Name,
                request.Slug,
                request.Description,
                request.IsActive);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            // Update in repository
            _assemblyMethodRepository.Update(assemblyMethod);

            return Result.Success();
        }, cancellationToken);
    }
}
