using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateInstockOrder;

internal sealed class CreateInstockOrderCommandHandler : ICommandTHandler<CreateInstockOrderCommand, Guid>
{
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInstockPriceRepository _priceRepository;
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockOrderCodeGenerator _codeGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateInstockOrderCommandHandler(
        IInstockOrderRepository orderRepository,
        IInstockProductVariantRepository variantRepository,
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInstockPriceRepository priceRepository,
        IInstockProductRepository productRepository,
        IInstockOrderCodeGenerator codeGenerator,
        IInStockUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _variantRepository = variantRepository;
        _priceDetailRepository = priceDetailRepository;
        _priceRepository = priceRepository;
        _productRepository = productRepository;
        _codeGenerator = codeGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.CartItems == null || !request.CartItems.Any())
        {
            return Result.Failure<Guid>(InstockOrderError.EmptyCart());
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Generate order code
            var orderCode = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);

            // Create order ID first
            var orderId = InstockOrderId.Create();

            // Process each cart item to create order details
            var orderDetails = new List<InstockOrderDetail>();
            decimal calculatedSubTotal = 0;

            foreach (var cartItem in request.CartItems)
            {
                // Get variant
                var variantId = InstockProductVariantId.From(cartItem.ItemId);
                var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);
                
                if (variant == null)
                {
                    return Result.Failure<Guid>(InstockOrderError.VariantNotFound(cartItem.ItemId));
                }

                if (!variant.IsActive)
                {
                    return Result.Failure<Guid>(InstockOrderError.VariantNotActive(variant.Sku));
                }

                // Get price detail
                var priceDetailId = InstockProductPriceDetailId.From(cartItem.PriceDetailId);
                var priceDetail = await _priceDetailRepository.GetByIdAsync(priceDetailId, cancellationToken);
                
                if (priceDetail == null)
                {
                    return Result.Failure<Guid>(InstockOrderError.PriceDetailNotFound(cartItem.PriceDetailId));
                }

                if (!priceDetail.IsActive)
                {
                    return Result.Failure<Guid>(InstockOrderError.PriceDetailNotActive(cartItem.PriceDetailId));
                }

                // Get price and validate it's active
                var price = await _priceRepository.GetByIdAsync(priceDetail.InstockPriceId, cancellationToken);
                
                if (price == null || !price.IsActive)
                {
                    return Result.Failure<Guid>(InstockOrderError.PriceNotActiveOrNotFound());
                }

                // Check if this price has the highest priority among prices assigned to this variant
                var variantPriceDetails = await _priceDetailRepository.GetAllByProductVariantIdAsync(variantId, cancellationToken);
                var activePrices = new List<InstockPrice>();
                
                foreach (var pd in variantPriceDetails.Where(pd => pd.IsActive))
                {
                    var pdPrice = await _priceRepository.GetByIdAsync(pd.InstockPriceId, cancellationToken);
                    if (pdPrice != null && pdPrice.IsActive)
                    {
                        activePrices.Add(pdPrice);
                    }
                }
                
                if (!activePrices.Any())
                {
                    return Result.Failure<Guid>(InstockOrderError.PriceNotHighestPriority());
                }
                
                // Find highest priority price for this variant
                var highestPriorityPrice = activePrices
                    .OrderByDescending(p => p.Priority)
                    .FirstOrDefault();
                
                if (highestPriorityPrice == null || price.Priority != highestPriorityPrice.Priority)
                {
                    return Result.Failure<Guid>(InstockOrderError.PriceNotHighestPriority());
                }

                // Get product for name
                var product = await _productRepository.GetByIdAsync(variant.InstockProductId, cancellationToken);
                string? productName = product?.Name;

                // Build variant name: Color + Dimensions
                var variantName = $"{variant.Color} {variant.AssembledLengthMm}x{variant.AssembledWidthMm}x{variant.AssembledHeightMm}";

                // Create order detail
                var orderDetailResult = InstockOrderDetail.Create(
                    orderId,
                    variantId,
                    variant.Sku,
                    priceDetail.UnitPrice,
                    cartItem.Quantity,
                    priceDetailId,
                    price.Name,
                    productName,
                    variantName);

                if (orderDetailResult.IsFailure)
                {
                    return Result.Failure<Guid>(orderDetailResult.Error);
                }

                var orderDetail = orderDetailResult.Value;
                orderDetails.Add(orderDetail);

                // Add to calculated subtotal
                calculatedSubTotal += orderDetail.TotalAmount;
            }

            // Validate grand total matches
            var usedCoinAmountAsMoney = request.UsedCoinAmount * 1000m; // Assuming 1 coin = 1000 VND
            var calculatedGrandTotal = calculatedSubTotal + request.ShippingFee - usedCoinAmountAsMoney;

            if (Math.Abs(calculatedGrandTotal - request.GrandTotalAmount) > 0.01m) // Allow small rounding difference
            {
                return Result.Failure<Guid>(InstockOrderError.GrandTotalMismatch(
                    calculatedGrandTotal,
                    request.GrandTotalAmount));
            }

            // Create the order
            var orderResult = InstockOrder.Create(
                orderCode,
                request.CustomerId,
                request.CustomerName,
                request.CustomerPhone,
                request.CustomerEmail,
                request.CustomerProvinceName,
                request.CustomerDistrictName,
                request.CustomerWardName,
                request.DetailAddress,
                calculatedSubTotal,
                request.ShippingFee,
                request.UsedCoinAmount,
                usedCoinAmountAsMoney,
                request.GrandTotalAmount,
                request.PaymentMethod);

            if (orderResult.IsFailure)
            {
                return Result.Failure<Guid>(orderResult.Error);
            }

            var order = orderResult.Value;

            // Add all order details to the order
            foreach (var detail in orderDetails)
            {
                order.AddOrderDetail(detail);
            }

            // Raise order created event with cart item IDs for clearing cart
            var cartItemIds = request.CartItems.Select(ci => ci.ItemId).ToList();
            order.RaiseOrderCreatedEvent(cartItemIds);

            // Save order (order details will be saved via navigation property)
            _orderRepository.Add(order);

            return Result.Success(order.Id.Value);
        }, cancellationToken);
    }
}
