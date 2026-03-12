using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailsByVariantId;

internal sealed class GetInstockProductPriceDetailsByVariantIdQueryHandler 
    : IQueryHandler<GetInstockProductPriceDetailsByVariantIdQuery, object>
{
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInstockPriceRepository _priceRepository;
    private readonly ICurrentUser _currentUser;

    public GetInstockProductPriceDetailsByVariantIdQueryHandler(
        IInstockProductVariantRepository variantRepository,
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInstockPriceRepository priceRepository,
        ICurrentUser currentUser)
    {
        _variantRepository = variantRepository;
        _priceDetailRepository = priceDetailRepository;
        _priceRepository = priceRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetInstockProductPriceDetailsByVariantIdQuery request,
        CancellationToken cancellationToken)
    {
        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);

        if (variant is null)
        {
            return Result.Failure<object>(InstockProductVariantError.NotFound(request.VariantId));
        }

        var priceDetails = await _priceDetailRepository.GetAllByProductVariantIdAsync(variantId, cancellationToken);
        var prices = await _priceRepository.GetAllAsync(cancellationToken);
        var priceDict = prices.ToDictionary(p => p.Id.Value);

        var isStaffOrManager = _currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager);

        if (isStaffOrManager)
        {
            // Staff/Manager: Return all price details
            var allPriceDetails = priceDetails
                .Select(pd => new PriceDetailDto(
                    pd.Id.Value,
                    pd.InstockPriceId.Value,
                    priceDict.TryGetValue(pd.InstockPriceId.Value, out var price) ? price.Name : "Unknown",
                    priceDict.TryGetValue(pd.InstockPriceId.Value, out var p) ? p.Priority : 0,
                    pd.UnitPrice,
                    pd.IsActive,
                    pd.CreatedAt,
                    pd.UpdatedAt))
                .ToList();

            return Result.Success<object>(allPriceDetails);
        }
        else
        {
            // Anonymous: Return only the active price detail with highest priority (higher number = higher priority)
            var activePriceDetail = priceDetails
                .Where(pd => pd.IsActive)
                .Where(pd => priceDict.ContainsKey(pd.InstockPriceId.Value) && priceDict[pd.InstockPriceId.Value].IsActive)
                .OrderByDescending(pd => priceDict[pd.InstockPriceId.Value].Priority)
                .FirstOrDefault();

            if (activePriceDetail is null)
            {
                return Result.Success<object>(null!);
            }

            var anonymousDto = new AnonymousPriceDetailDto(
                activePriceDetail.Id.Value,
                activePriceDetail.InstockPriceId.Value,
                priceDict[activePriceDetail.InstockPriceId.Value].Name,
                activePriceDetail.UnitPrice);

            return Result.Success<object>(anonymousDto);
        }
    }
}
