using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrderById;

internal sealed class GetCustomerOrderByIdQueryHandler 
    : IQueryHandler<GetCustomerOrderByIdQuery, GetCustomerOrderByIdResponseDto>
{
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _assetUrlService;

    public GetCustomerOrderByIdQueryHandler(
        IInstockOrderRepository orderRepository,
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository,
        ICurrentUser currentUser,
        IMediaAssetService assetUrlService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _currentUser = currentUser;
        _assetUrlService = assetUrlService;
    }

    public async Task<ResultT<GetCustomerOrderByIdResponseDto>> Handle(
        GetCustomerOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrEmpty(_currentUser.UserId))
            return Result.Failure<GetCustomerOrderByIdResponseDto>(
                InstockOrderError.Unauthorized());

        var customerId = Guid.Parse(_currentUser.UserId);
        var orderId = InstockOrderId.From(request.OrderId);
        
        var order = await _orderRepository.GetByIdWithDetailsAsync(orderId, cancellationToken);

        if (order == null)
            return Result.Failure<GetCustomerOrderByIdResponseDto>(
                InstockOrderError.NotFound(request.OrderId));

        //// Verify order belongs to current customer
        //if (order.CustomerId != customerId)
        //    return Result.Failure<GetCustomerOrderByIdResponseDto>(
        //        InstockOrderError.Unauthorized());

        // Get all variant IDs and product IDs
        var variantIds = order.OrderDetails.Select(od => od.InstockProductVariantId.Value).Distinct().ToList();
        
        // Get all variants
        var allVariants = await _variantRepository.GetAllAsync(cancellationToken);
        var variantsMap = allVariants
            .Where(v => variantIds.Contains(v.Id.Value))
            .ToDictionary(v => v.Id.Value, v => v);

        // Get product IDs from variants
        var productIds = variantsMap.Values.Select(v => v.InstockProductId.Value).Distinct().ToList();
        
        // Get all products
        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        var productsMap = allProducts
            .Where(p => productIds.Contains(p.Id.Value))
            .ToDictionary(p => p.Id.Value, p => p);

        // Map thumbnails by variant ID
        var variantToThumbnailMap = variantsMap.Values.ToDictionary(
            v => v.Id.Value,
            v => productsMap.TryGetValue(v.InstockProductId.Value, out var product) 
                ? _assetUrlService.BuildAssetUrl(product.ThumbnailUrl)
                : null);

        var orderDetails = order.OrderDetails.Select(od =>
        {
            variantsMap.TryGetValue(od.InstockProductVariantId.Value, out var variant);
            var productId = variant?.InstockProductId.Value;
            productsMap.TryGetValue(productId ?? Guid.Empty, out var product);

            return new OrderDetailFullDto(
                od.Id.Value,
                od.InstockProductVariantId.Value,
                od.Sku,
                od.ProductName,
                od.VariantName,
                od.UnitPrice,
                od.Quantity,
                od.TotalAmount,
                od.PriceName,
                variantToThumbnailMap.TryGetValue(od.InstockProductVariantId.Value, out var thumbnail) 
                    ? thumbnail 
                    : null,
                product != null 
                    ? new ProductFullDetailsDto(
                        product.Id.Value,
                        product.Code,
                        product.Name,
                        product.Description,
                        product.DifficultLevel,
                        product.EstimatedBuildTime,
                        product.TotalPieceCount,
                        _assetUrlService.BuildAssetUrl(product.ThumbnailUrl),
                        _assetUrlService.BuildAssetUrls(product.PreviewAsset),
                        product.IsActive)
                    : null,
                variant != null
                    ? new VariantFullDetailsDto(
                        variant.Color,
                        variant.AssembledLengthMm,
                        variant.AssembledWidthMm,
                        variant.AssembledHeightMm,
                        variant.IsActive)
                    : null);
        }).ToList();

        var response = new GetCustomerOrderByIdResponseDto(
            order.Id.Value,
            order.Code,
            order.CustomerName,
            order.CustomerPhone,
            order.CustomerEmail,
            order.CustomerProvinceName,
            order.CustomerDistrictName,
            order.CustomerWardName,
            order.DetailAddress,
            order.SubTotalAmount,
            order.ShippingFee,
            order.UsedCoinAmount,
            order.GrandTotalAmount,
            order.Status.ToString(),
            order.PaymentMethod,
            order.IsPaid,
            order.PaidAt,
            order.CreatedAt,
            order.UpdatedAt,
            orderDetails);

        return Result.Success(response);
    }
}




