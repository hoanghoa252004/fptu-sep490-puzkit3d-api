using System.Text.Json;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.CreateInstockProduct;

internal sealed class CreateInstockProductCommandHandler : ICommandTHandler<CreateInstockProductCommand, Guid>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductCodeGenerator _codeGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;

    public CreateInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInstockProductCodeGenerator codeGenerator,
        IInStockUnitOfWork unitOfWork,
        ITopicReplicaRepository topicReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository)
    {
        _productRepository = productRepository;
        _codeGenerator = codeGenerator;
        _unitOfWork = unitOfWork;
        _topicReplicaRepository = topicReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockProductCommand request, CancellationToken cancellationToken)
    {
        var existingBySlug = await _productRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingBySlug is not null)
        {
            return Result.Failure<Guid>(InstockProductError.DuplicateSlug(request.Slug));
        }

        // Validate at least 1 capability provided
        if (request.CapabilityIds == null || request.CapabilityIds.Count == 0)
        {
            return Result.Failure<Guid>(InstockProductError.InvalidCapability());
        }

        // Validate foreign keys exist in replicas
        var topicExists = await _topicReplicaRepository.ExistsByIdAsync(request.TopicId, cancellationToken);
        if (!topicExists)
        {
            return Result.Failure<Guid>(InstockProductError.InvalidTopic());
        }

        var assemblyMethodExists = await _assemblyMethodReplicaRepository.ExistsByIdAsync(request.AssemblyMethodId, cancellationToken);
        if (!assemblyMethodExists)
        {
            return Result.Failure<Guid>(InstockProductError.InvalidAssemblyMethod());
        }

        // Validate all capabilities exist
        foreach (var capabilityId in request.CapabilityIds)
        {
            var capabilityExists = await _capabilityReplicaRepository.ExistsByIdAsync(capabilityId, cancellationToken);
            if (!capabilityExists)
            {
                return Result.Failure<Guid>(InstockProductError.InvalidCapability());
            }
        }

        var materialExists = await _materialReplicaRepository.ExistsByIdAsync(request.MaterialId, cancellationToken);
        if (!materialExists)
        {
            return Result.Failure<Guid>(InstockProductError.InvalidMaterial());
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);
            var previewAssetJson = JsonSerializer.Serialize(request.PreviewAsset);

            // Use first capability as primary, others will be added via AddCapability
            var primaryCapabilityId = request.CapabilityIds[0];

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
                request.AssemblyMethodId,
                primaryCapabilityId,
                request.MaterialId,
                request.Description,
                request.IsActive);

            if (productResult.IsFailure)
            {
                return Result.Failure<Guid>(productResult.Error);
            }

            var product = productResult.Value;

            // Add remaining capabilities
            for (int i = 1; i < request.CapabilityIds.Count; i++)
            {
                product.AddCapability(request.CapabilityIds[i]);
            }

            _productRepository.Add(product);

            return Result.Success(product.Id.Value);
        }, cancellationToken);
    }
}



