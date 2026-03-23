//using PuzKit3D.Modules.Delivery.Application.Services;
//using PuzKit3D.Modules.InStock.Application.Repositories;
//using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
//using PuzKit3D.SharedKernel.Application.Message.Query;
//using PuzKit3D.SharedKernel.Domain.Errors;
//using PuzKit3D.SharedKernel.Domain.Results;
//using System.Text.Json;

//namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateDeliverTrackingOrder;

//internal sealed class CreateDeliverTrackingOrderCommandHandler : IQueryHandler<CreateDeliverTrackingOrderCommand, CreateDeliverTrackingOrderResponseDto>
//{
//    private readonly IInstockOrderRepository _orderRepository;
//    private readonly IDeliveryService _deliveryService;

//    public CreateDeliverTrackingOrderCommandHandler(
//        IInstockOrderRepository orderRepository,
//        IDeliveryService deliveryService)
//    {
//        _orderRepository = orderRepository;
//        _deliveryService = deliveryService;
//    }

//    public async Task<ResultT<CreateDeliverTrackingOrderResponseDto>> Handle(CreateDeliverTrackingOrderCommand request, CancellationToken cancellationToken)
//    {
//        // Validate order exists with details
//        var orderId = InstockOrderId.From(request.OrderId);
//        var order = await _orderRepository.GetByIdWithDetailsAsync(orderId, cancellationToken);

//        if (order == null)
//        {
//            return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                Error.NotFound("INSTOCK_ORDER_NOT_FOUND", $"Order with ID {request.OrderId} not found"));
//        }

//        // Check if order status is Processing
//        if (order.Status != InstockOrderStatus.Processing)
//        {
//            return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                InstockOrderError.InvalidStatusTransition(order.Status, InstockOrderStatus.Processing));
//        }

//        // Check if delivery info already exists
//        if (order.DeliveryOrderCode != null && order.ExpectedDeliveryDate != null)
//        {
//            return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                InstockOrderError.DeliveryInfoAlreadySet());
//        }

//        // Build shipping items from order details
//        var items = order.OrderDetails.Select(detail => new Delivery.Application.DTOs.ShippingOrderItem
//        {
//            Name = $"{detail.ProductName ?? "Product"} - {detail.VariantName ?? "Variant"}",
//            Code = detail.Sku,
//            Quantity = detail.Quantity,
//            Price = (int)detail.UnitPrice
//        }).ToList();

//        // Create shipping order with GHN
//        var shippingRequest = new Delivery.Application.DTOs.CreateShippingOrderRequest
//        {
//            ToName = order.CustomerName,
//            ToPhone = order.CustomerPhone,
//            ToAddress = order.DetailAddress,
//            ToWardName = order.CustomerWardName,
//            ToDistrictName = order.CustomerDistrictName,
//            ToProvinceName = order.CustomerProvinceName,
//            OrderCode = order.Id.Value.ToString(),
//            RequiredNote = "CHOXEMHANGKHONGTHU",
//            Note = $"Order {order.Code}",
//            Items = items,
//            Content = "Puzzle 3D Product",
//            CodAmount =  string.Equals(order.PaymentMethod, "COD", StringComparison.OrdinalIgnoreCase) ? (int)order.SubTotalAmount : 0
//        };

//        var result = await _deliveryService.CreateShippingOrderAsync(shippingRequest, cancellationToken);

//        if (result.IsFailure)
//        {
//            return Result.Failure<CreateDeliverTrackingOrderResponseDto>(result.Error);
//        }

//        // Parse GHN response
//        var ghnResponse = result.Value;
        
//        try
//        {
//            // Convert object to JsonElement first
//            var jsonElement = JsonSerializer.Serialize(ghnResponse);
//            var jsonDocument = JsonDocument.Parse(jsonElement);
//            var root = jsonDocument.RootElement;

//            // Extract data object
//            if (!root.TryGetProperty("data", out var dataElement))
//            {
//                return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                    GhnOrderError.InvalidGhnResponse("Missing 'data' property in GHN response"));
//            }

//            // Extract order_code
//            if (!dataElement.TryGetProperty("order_code", out var orderCodeElement))
//            {
//                return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                    GhnOrderError.InvalidGhnResponse("Missing 'order_code' in GHN data"));
//            }

//            // Extract sort_code
//            if (!dataElement.TryGetProperty("sort_code", out var sortCodeElement))
//            {
//                return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                    GhnOrderError.InvalidGhnResponse("Missing 'sort_code' in GHN data"));
//            }

//            // Extract expected_delivery_time
//            if (!dataElement.TryGetProperty("expected_delivery_time", out var expectedDeliveryElement))
//            {
//                return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                    GhnOrderError.InvalidGhnResponse("Missing 'expected_delivery_time' in GHN data"));
//            }

//            var orderCode = orderCodeElement.GetString() ?? string.Empty;
//            var sortCode = sortCodeElement.GetString() ?? string.Empty;
            
//            if (!DateTime.TryParse(expectedDeliveryElement.GetString(), out var expectedDeliveryTime))
//            {
//                return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                    GhnOrderError.InvalidGhnResponse("Invalid 'expected_delivery_time' format"));
//            }

//            var response = new CreateDeliverTrackingOrderResponseDto(orderCode, expectedDeliveryTime);

//            return Result.Success(response);
//        }
//        catch (Exception ex)
//        {
//            return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
//                GhnOrderError.ParsingGhnResponseFailed(ex.Message));
//        }
//    }
//}

///// <summary>
///// Error handling for GHN operations
///// </summary>
//internal static class GhnOrderError
//{
//    public static Error InvalidGhnResponse(string message) =>
//        Error.Failure("GHN_INVALID_RESPONSE", $"Invalid GHN response: {message}");

//    public static Error ParsingGhnResponseFailed(string message) =>
//        Error.Failure("GHN_PARSING_FAILED", $"Failed to parse GHN response: {message}");
//}
