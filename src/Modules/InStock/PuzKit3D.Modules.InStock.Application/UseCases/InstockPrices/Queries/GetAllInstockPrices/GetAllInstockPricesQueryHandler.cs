using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetAllInstockPrices;

internal sealed class GetAllInstockPricesQueryHandler : IQueryHandler<GetAllInstockPricesQuery, PagedResult<object>>
{
    private readonly IInstockPriceRepository _priceRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllInstockPricesQueryHandler(
        IInstockPriceRepository priceRepository,
        ICurrentUser currentUser)
    {
        _priceRepository = priceRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllInstockPricesQuery request,
        CancellationToken cancellationToken)
    {
        var prices = await _priceRepository.GetAllAsync(cancellationToken);

        var isStaffOrManager = _currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager);

        if (!isStaffOrManager)
        {
            prices = prices.Where(p => p.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            prices = prices.Where(p => p.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (request.IsActive.HasValue)
        {
            prices = prices.Where(p => p.IsActive == request.IsActive.Value);
        }

        // Sort by CreatedAt ascending
        prices = prices.OrderBy(p => p.CreatedAt);

        var totalCount = prices.Count();

        var priceDtos = prices
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p =>
            {
                if (isStaffOrManager)
                {
                    return (object)new GetAllInstockPricesResponseDto(
                        p.Id.Value,
                        p.Name,
                        p.EffectiveFrom,
                        p.EffectiveTo,
                        p.Priority,
                        p.IsActive,
                        p.CreatedAt,
                        p.UpdatedAt);
                }
                else
                {
                    return new AnonymousInstockPriceDto(
                        p.Id.Value,
                        p.Name,
                        p.EffectiveFrom,
                        p.EffectiveTo,
                        p.Priority);
                }
            })
            .ToList();

        var pagedResult = new PagedResult<object>(
            priceDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
