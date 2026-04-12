using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductBySlug;

internal sealed class GetInstockProductBySlugQueryHandler 
    : IQueryHandler<GetInstockProductBySlugQuery, GetInstockProductBySlugResponseDto>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IDriveReplicaRepository _driveReplicaRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _assetUrlService;

    public GetInstockProductBySlugQueryHandler(
        IInstockProductRepository productRepository,
        IDriveReplicaRepository driveReplicaRepository,
        ICurrentUser currentUser,
        IMediaAssetService assetUrlService)
    {
        _productRepository = productRepository;
        _driveReplicaRepository = driveReplicaRepository;
        _currentUser = currentUser;
        _assetUrlService = assetUrlService;
    }

    public async Task<ResultT<GetInstockProductBySlugResponseDto>> Handle(
        GetInstockProductBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (product is null)
        {
            return Result.Failure<GetInstockProductBySlugResponseDto>(
                InstockProductError.NotFoundBySlug(request.Slug));
        }

        // Check if user is Staff or Business Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // For anonymous/customer users, only show active products
        if (!isStaffOrManager && !product.IsActive)
        {
            return Result.Failure<GetInstockProductBySlugResponseDto>(
                Error.NotFound(
                    "InstockProduct.NotActive",
                    $"Instock product with slug '{request.Slug}' is not available."));
        }

        // Get drive replicas to map names
        var driveIds = product.Drives.Select(d => d.DriveId).ToList();
        var driveReplicas = (await _driveReplicaRepository.GetByIdsAsync(driveIds, cancellationToken))
            .ToDictionary(d => d.Id);

        var driveDetails = product.Drives
            .Select(d => new GetInstockProductBySlugDriveDetailDto(
                d.DriveId,
                driveReplicas.TryGetValue(d.DriveId, out var replica) ? replica.Name : "Unknown",
                d.Quantity))
            .ToList();

        var response = new GetInstockProductBySlugResponseDto(
            product.Id.Value,
            product.Code,
            product.Slug,
            product.Name,
            product.TotalPieceCount,
            product.DifficultLevel,
            product.EstimatedBuildTime,
            _assetUrlService.BuildAssetUrl(product.ThumbnailUrl),
            _assetUrlService.BuildAssetUrls(product.PreviewAsset),
            product.Description,
            product.TopicId,
            product.AssemblyMethodId,
            product.GetCapabilityIds(),
            product.MaterialId,
            driveDetails,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);

        return Result.Success(response);
    }
}






