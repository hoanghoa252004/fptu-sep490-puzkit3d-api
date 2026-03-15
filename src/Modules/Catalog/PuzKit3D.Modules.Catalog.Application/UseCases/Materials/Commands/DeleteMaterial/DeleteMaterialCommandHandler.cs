using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.DeleteMaterial;

internal sealed class DeleteMaterialCommandHandler : ICommandHandler<DeleteMaterialCommand>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteMaterialCommandHandler(
        IMaterialRepository materialRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        // Get material by ID
        var materialId = MaterialId.From(request.Id);
        var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);

        if (material is null)
        {
            return Result.Failure(MaterialError.NotFound(request.Id));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            material.Delete();
            // Delete from repository
            _materialRepository.Delete(material);

            return Result.Success();
        }, cancellationToken);
    }
}
