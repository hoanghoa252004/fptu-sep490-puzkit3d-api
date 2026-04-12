using System.Text.Json;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.CreateInstockProduct;

internal sealed class CreateInstockProductCommandHandler : ICommandTHandler<CreateInstockProductCommand, Guid>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductDriveRepository _productDriveRepository;
    private readonly IInstockProductCodeGenerator _codeGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;
    private readonly IDriveReplicaRepository _driveReplicaRepository;

    public CreateInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInstockProductDriveRepository productDriveRepository,
        IInstockProductCodeGenerator codeGenerator,
        IInStockUnitOfWork unitOfWork,
        ITopicReplicaRepository topicReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository,
        IDriveReplicaRepository driveReplicaRepository)
    {
        _productRepository = productRepository;
        _productDriveRepository = productDriveRepository;
        _codeGenerator = codeGenerator;
        _unitOfWork = unitOfWork;
        _topicReplicaRepository = topicReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
        _driveReplicaRepository = driveReplicaRepository;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockProductCommand request, CancellationToken cancellationToken)
    {
        var existingBySlug = await _productRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingBySlug is not null)
        {
            return Result.Failure<Guid>(InstockProductError.DuplicateSlug(request.Slug));
        }

        // Validate foreign keys exist in replicas
        var topicExists = await _topicReplicaRepository.ExistsByIdAsync(request.TopicId, cancellationToken);
        if (!topicExists)
        {
            return Result.Failure<Guid>(InstockProductError.InvalidTopic());
        }

        var materialExists = await _materialReplicaRepository.ExistsByIdAsync(request.MaterialId, cancellationToken);
        if (!materialExists)
        {
            return Result.Failure<Guid>(InstockProductError.InvalidMaterial());
        }

        // Validate capabilities exist in replica if provided
        if (request.CapabilityIds != null && request.CapabilityIds.Count > 0)
        {
            foreach (var capabilityId in request.CapabilityIds)
            {
                var capabilityExists = await _capabilityReplicaRepository.ExistsByIdAsync(capabilityId, cancellationToken);
                if (!capabilityExists)
                {
                    return Result.Failure<Guid>(InstockProductError.InvalidCapability(capabilityId));
                }
            }
        }

        // Validate assembly methods exist in replica if provided (new validation)
        if (request.AssemblyMethodIds != null && request.AssemblyMethodIds.Count > 0)
        {
            foreach (var assemblyMethodId in request.AssemblyMethodIds)
            {
                var additionalAssemblyMethodExists = await _assemblyMethodReplicaRepository.ExistsByIdAsync(assemblyMethodId, cancellationToken);
                if (!additionalAssemblyMethodExists)
                {
                    return Result.Failure<Guid>(InstockProductError.InvalidAssemblyMethod());
                }
            }
        }

        // Validate drive details if provided
        if (request.DriveDetails != null && request.DriveDetails.Count > 0)
        {
            foreach (var driveDetail in request.DriveDetails)
            {

                var driveExists = await _driveReplicaRepository.ExistsByIdAsync(driveDetail.DriveId, cancellationToken);
                if (!driveExists)
                {
                    return Result.Failure<Guid>(InstockProductError.InvalidDrive(driveDetail.DriveId));
                }

                if (driveDetail.Quantity <= 0)
                {
                    return Result.Failure<Guid>(InstockProductError.InvalidQuantity());
                }
            }
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);
            var previewAssetJson = JsonSerializer.Serialize(request.PreviewAsset);

            var productResult = InstockProduct.Create(
                code,
                request.Slug,
                request.Name,
                request.TotalPieceCount,
                request.DifficultLevel,
                request.EstimatedBuildTime,
                request.ThumbnailUrl,
                previewAssetJson,
                request.TopicId,
                request.MaterialId,
                request.Description,
                request.IsActive);

            if (productResult.IsFailure)
            {
                return Result.Failure<Guid>(productResult.Error);
            }

            var product = productResult.Value;

            // Set capability details if provided
            if (request.CapabilityIds != null && request.CapabilityIds.Count > 0)
            {
                product.SetCapabilities(request.CapabilityIds);
            }

            // Set assembly method details if provided
            if (request.AssemblyMethodIds != null && request.AssemblyMethodIds.Count > 0)
            {
                product.SetAssemblyMethods(request.AssemblyMethodIds);
            }

            // Add drives if provided
            if (request.DriveDetails != null && request.DriveDetails.Count > 0)
            {
                foreach (var driveDetail in request.DriveDetails)
                {
                    var drive = InstockProductDrive.Create(
                        product.Id,
                        driveDetail.DriveId,
                        driveDetail.Quantity);

                    _productDriveRepository.Add(drive);
                }
            }

            _productRepository.Add(product);

            return Result.Success(product.Id.Value);
        }, cancellationToken);
    }
}



