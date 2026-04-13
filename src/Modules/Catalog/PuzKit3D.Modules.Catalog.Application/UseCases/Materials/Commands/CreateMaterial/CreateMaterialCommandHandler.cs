using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.CreateMaterial;

internal sealed class CreateMaterialCommandHandler : ICommandTHandler<CreateMaterialCommand, Guid>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateMaterialCommandHandler(
        IMaterialRepository materialRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        // Check if slug already exists
        var existingMaterial = await _materialRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (existingMaterial is not null)
        {
            return Result.Failure<Guid>(MaterialError.DuplicateSlug(request.Slug));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create material using factory method
            var materialResult = Material.Create(
                request.Name,
                request.Slug,
                request.FactorPercentage,
                request.BasePrice,
                request.Description,
                request.IsActive);

            if (materialResult.IsFailure)
            {
                return Result.Failure<Guid>(materialResult.Error);
            }

            var material = materialResult.Value;

            // Add to repository
            _materialRepository.Add(material);

            return Result.Success(material.Id.Value);
        }, cancellationToken);
    }
}
