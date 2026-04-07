using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductsByPartnerId;

internal sealed class GetPartnerProductsByPartnerIdQueryHandler
    : IQueryHandler<GetPartnerProductsByPartnerIdQuery, PagedResult<object>>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _assetUrlService;

    public GetPartnerProductsByPartnerIdQueryHandler(
        IPartnerProductRepository partnerProductRepository,
        ICurrentUser currentUser,
        IMediaAssetService assetUrlService)
    {
        _partnerProductRepository = partnerProductRepository;
        _currentUser = currentUser;
        _assetUrlService = assetUrlService;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetPartnerProductsByPartnerIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get all partner products filtered by partner ID
        var allProducts = await _partnerProductRepository.GetAllByPartnerIdAsync(
            request.PartnerId,
            isStaffOrManager,
            request.SearchTerm,
            request.Ascending,
            cancellationToken);

        // Apply pagination
        var totalCount = allProducts.Count();
        var products = allProducts
            .OrderBy(p => p.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        IReadOnlyList<object> productDtos;
        if (isStaffOrManager)
        {
            productDtos = products
                .Select(p => (object)new GetPartnerProductsByPartnerIdResponseDto(
                    p.Id.Value,
                    p.PartnerId.Value,
                    p.Name,
                    p.ReferencePrice,
                    p.Quantity,
                    _assetUrlService.BuildAssetUrl(p.ThumbnailUrl),
                    _assetUrlService.BuildAssetUrls(p.PreviewAsset),
                    p.Slug,
                    p.Description,
                    p.IsActive,
                    p.CreatedAt,
                    p.UpdatedAt))
                .ToList();
        }
        else
        {
            productDtos = products
                .Select(p => (object)new GetPartnerProductsByPartnerIdPublicResponseDto(
                    p.Id.Value,
                    p.PartnerId.Value,
                    p.Name,
                    p.ReferencePrice,
                    _assetUrlService.BuildAssetUrl(p.ThumbnailUrl),
                    _assetUrlService.BuildAssetUrls(p.PreviewAsset),
                    p.Slug,
                    p.Description))
                .ToList();
        }

        var pagedResult = new PagedResult<object>(
            productDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
