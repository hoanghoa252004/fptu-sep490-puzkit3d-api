using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.Modules.Catalog.Domain.UnitOfWork;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.CreateAssemblyMethod;

internal sealed class CreateAssemblyMethodCommandHandler : ICommandTHandler<CreateAssemblyMethodCommand, Guid>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateAssemblyMethodCommandHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateAssemblyMethodCommand request, CancellationToken cancellationToken)
    {
        // Check if slug already exists
        var existingAssemblyMethod = await _assemblyMethodRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (existingAssemblyMethod is not null)
        {
            return Result.Failure<Guid>(AssemblyMethodError.DuplicateSlug(request.Slug));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create assembly method using factory method
            var assemblyMethodResult = AssemblyMethod.Create(
                request.Name,
                request.Slug,
                request.Description,
                request.IsActive);

            if (assemblyMethodResult.IsFailure)
            {
                return Result.Failure<Guid>(assemblyMethodResult.Error);
            }

            var assemblyMethod = assemblyMethodResult.Value;

            // Add to repository
            _assemblyMethodRepository.Add(assemblyMethod);

            return Result.Success(assemblyMethod.Id.Value);
        }, cancellationToken);
    }
}

