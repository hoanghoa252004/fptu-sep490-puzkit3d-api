using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrders;

internal sealed class GetCustomerOrdersQueryHandler 
    : IQueryHandler<GetCustomerOrdersQuery, PagedResult<GetCustomerOrdersResponseDto>>
{
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly ICurrentUser _currentUser;

    public GetCustomerOrdersQueryHandler(
        IInstockOrderRepository orderRepository,
        IInstockProductVariantRepository variantRepository,
        ICurrentUser currentUser)
    {
        _orderRepository = orderRepository;
        _variantRepository = variantRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<GetCustomerOrdersResponseDto>>> Handle(
        GetCustomerOrdersQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrEmpty(_currentUser.UserId))
            return Result.Failure<PagedResult<GetCustomerOrdersResponseDto>>(
                InstockOrderError.Unauthorized());

        var customerId = Guid.Parse(_currentUser.UserId);
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId, cancellationToken);

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
                .Select(od => new OrderDetailPreviewDto(
                    od.ProductName,
                    od.VariantName,
                    od.Quantity,
                    od.UnitPrice,
                    productThumbnails.TryGetValue(od.InstockProductVariantId.Value, out var thumbnail) 
                        ? thumbnail 
                        : null))
                .ToList();

            return new GetCustomerOrdersResponseDto(
                o.Id.Value,
                o.Code,
                o.GrandTotalAmount,
                totalQuantity,
                o.Status,
                o.PaymentMethod,
                o.IsPaid,
                o.PaidAt,
                o.CreatedAt,
                previewDetails);
        }).ToList();

        var pagedResult = new PagedResult<GetCustomerOrdersResponseDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
