using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.UpdateMaterial;

internal sealed class UpdateMaterialCommandHandler : ICommandHandler<UpdateMaterialCommand>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateMaterialCommandHandler(
        IMaterialRepository materialRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        // Get material by ID
        var materialId = MaterialId.From(request.Id);
        var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);

        if (material is null)
        {
            return Result.Failure(MaterialError.NotFound(request.Id));
        }

        // Check if slug is being changed to an existing one
        if (material.Slug != request.Slug)
        {
            var existingMaterial = await _materialRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingMaterial is not null)
            {
                return Result.Failure(MaterialError.DuplicateSlug(request.Slug));
            }
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Update material
            var updateResult = material.Update(
                request.Name,
                request.Slug,
                request.Description,
                request.IsActive);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            // Update in repository
            _materialRepository.Update(material);

            return Result.Success();
        }, cancellationToken);
    }
}
