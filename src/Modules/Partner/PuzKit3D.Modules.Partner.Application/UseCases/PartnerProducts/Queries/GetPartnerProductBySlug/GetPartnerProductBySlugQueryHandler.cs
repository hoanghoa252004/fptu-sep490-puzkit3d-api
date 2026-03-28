using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductBySlug;

internal sealed class GetPartnerProductBySlugQueryHandler : IQueryHandler<GetPartnerProductBySlugQuery, object>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _assetUrlService;

    public GetPartnerProductBySlugQueryHandler(
        IPartnerProductRepository partnerProductRepository,
        ICurrentUser currentUser,
        IMediaAssetService assetUrlService)
    {
        _partnerProductRepository = partnerProductRepository;
        _currentUser = currentUser;
        _assetUrlService = assetUrlService;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _partnerProductRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (product is null)
        {
            return Result.Failure<object>(
                PartnerProductError.NotFoundBySlug(request.Slug));
        }

        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // For non-staff/manager users, only show active products
        if (!isStaffOrManager && !product.IsActive)
        {
            return Result.Failure<object>(
                Error.NotFound(
                    "PartnerProduct.NotActive",
                    $"Partner product with slug '{request.Slug}' is not available."));
        }

        // Build response DTO based on user role
        object response = isStaffOrManager
            ? new GetPartnerProductBySlugResponseDto
            (
                product.Id.Value,
                product.PartnerId.Value,
                product.Name,
                product.ReferencePrice,
                product.Quantity,
                _assetUrlService.BuildAssetUrl(product.ThumbnailUrl),
                _assetUrlService.BuildAssetUrls(product.PreviewAsset),
                product.Slug,
                product.Description,
                product.IsActive,
                product.CreatedAt,
                product.UpdatedAt
            )
            : new GetPartnerProductBySlugForCustomerResponseDto
            (
                product.Id.Value,
                product.PartnerId.Value,
                product.Name,
                product.ReferencePrice,
                _assetUrlService.BuildAssetUrl(product.ThumbnailUrl),
                _assetUrlService.BuildAssetUrls(product.PreviewAsset),
                product.Slug,
                product.Description
            );

        return Result.Success(response);
    }
}
