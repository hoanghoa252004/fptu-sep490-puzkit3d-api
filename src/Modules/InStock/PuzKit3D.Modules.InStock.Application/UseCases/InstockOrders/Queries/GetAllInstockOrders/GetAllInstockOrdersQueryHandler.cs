using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetAllInstockOrders;

internal sealed class GetAllInstockOrdersQueryHandler 
    : IQueryHandler<GetAllInstockOrdersQuery, PagedResult<GetAllInstockOrdersResponseDto>>
{
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IAssetUrlService _assetUrlService;

    public GetAllInstockOrdersQueryHandler(
        IInstockOrderRepository orderRepository,
        IInstockProductVariantRepository variantRepository,
        IAssetUrlService assetUrlService)
    {
        _orderRepository = orderRepository;
        _variantRepository = variantRepository;
        _assetUrlService = assetUrlService;
    }

    public async Task<ResultT<PagedResult<GetAllInstockOrdersResponseDto>>> Handle(
        GetAllInstockOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllOrdersAsync(cancellationToken);

        var query = orders.AsQueryable();

        // Apply status filter if provided
        if (request.Status.HasValue)
        {
            query = query.Where(o => o.Status == request.Status.Value);
        }

        // Order by CreatedAt descending (newest first)
        query = query.OrderByDescending(o => o.CreatedAt);

        var totalCount = query.Count();

        var pagedOrders = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Get all variant IDs from order details (first 4 items per order)
        var variantIds = pagedOrders
            .SelectMany(o => o.OrderDetails.Take(4))
            .Select(od => od.InstockProductVariantId.Value)
            .Distinct()
            .ToList();

        // Load product thumbnails by variant IDs
        var productThumbnails = variantIds.Any()
            ? await _variantRepository.GetProductThumbnailsByVariantIdsAsync(variantIds, cancellationToken)
            : new Dictionary<Guid, string>();

        var items = pagedOrders.Select(o =>
        {
            var totalQuantity = o.OrderDetails.Sum(od => od.Quantity);
            var previewDetails = o.OrderDetails
                .Take(4)
                .Select(od => new AllOrderDetailPreviewDto(
                    od.ProductName,
                    od.VariantName,
                    od.Quantity,
                    od.UnitPrice,
                    productThumbnails.TryGetValue(od.InstockProductVariantId.Value, out var thumbnail) 
                        ? _assetUrlService.BuildAssetUrl(thumbnail)
                        : null))
                .ToList();

            return new GetAllInstockOrdersResponseDto(
                o.Id.Value,
                o.Code,
                o.CustomerId,
                o.CustomerName,
                o.CustomerPhone,
                o.GrandTotalAmount,
                totalQuantity,
                o.Status.ToString(),
                o.PaymentMethod,
                o.IsPaid,
                o.PaidAt,
                o.CreatedAt,
                previewDetails);
        }).ToList();

        var pagedResult = new PagedResult<GetAllInstockOrdersResponseDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}


