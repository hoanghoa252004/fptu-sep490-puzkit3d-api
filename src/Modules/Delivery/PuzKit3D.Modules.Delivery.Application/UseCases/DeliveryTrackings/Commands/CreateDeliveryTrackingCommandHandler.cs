using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;

public sealed class CreateDeliveryTrackingCommandHandler : ICommandTHandler<CreateDeliveryTrackingCommand, Guid>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IDeliveryService _deliveryService;
    private readonly IOrderDetailReplicaRepository _orderDetailReplicaRepository;

    public CreateDeliveryTrackingCommandHandler(
        IDeliveryTrackingRepository deliveryTrackingRepository,
        IOrderReplicaRepository orderReplicaRepository,
        IDeliveryService deliveryService,
        IOrderDetailReplicaRepository orderDetailReplicaRepository)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _deliveryService = deliveryService;
        _orderReplicaRepository = orderReplicaRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
    }

    public async Task<ResultT<Guid>> Handle(CreateDeliveryTrackingCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if order exists
        var orderResult = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (orderResult.IsFailure)
        {
            return Result.Failure<Guid>(orderResult.Error);
        }

        var order = orderResult.Value;

        // 2. Check if DeliveryTracking already exists for this order
        var existingTrackings = await _deliveryTrackingRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
        var trackingType = !existingTrackings.Any() ? DeliveryTrackingType.Original : DeliveryTrackingType.Support;

        // 3. Get order details
        var orderDetails = await _orderDetailReplicaRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
        if (!orderDetails.Any())
        {
            return Result.Failure<Guid>(OrderReplicaError.OrderDetailNotFound(request.OrderId));
        }

        // 4. Build shipping items from order details
        var items = orderDetails.Select(detail => new ShippingOrderItem
        {
            Name = $"Product {detail.ProductId}",
            Code = (detail.VariantId ?? detail.ProductId).ToString(),
            Quantity = detail.Quantity,
            Price = 0
        }).ToList();

        // 5. Create shipping order request
        var shippingRequest = new CreateShippingOrderRequest
        {
            ToName = "Customer",
            ToPhone = "0000000000",
            ToAddress = "Address",
            ToWardName = "Ward",
            ToDistrictName = "District",
            ToProvinceName = "Province",
            OrderCode = order.Id.ToString(),
            RequiredNote = "CHOXEMHANGKHONGTHU",
            Note = $"Order {order.Code}",
            Items = items,
            Content = "Puzzle 3D Product",
            CodAmount = 0
        };

        // 6. Create shipping order with delivery service
        var ghnResult = await _deliveryService.CreateShippingOrderAsync(shippingRequest, cancellationToken);
        if (ghnResult.IsFailure)
        {
            return Result.Failure<Guid>(ghnResult.Error);
        }

        // 7. Parse GHN response to extract delivery order code and expected delivery date
        var (deliveryOrderCode, expectedDeliveryDate) = ParseGhnResponse(ghnResult.Value);

        // 8. Create DeliveryTracking entity
        var createTrackingResult = DeliveryTracking.Create(
            order.Id,
            deliveryOrderCode,
            expectedDeliveryDate,
            trackingType);

        if (createTrackingResult.IsFailure)
        {
            return Result.Failure<Guid>(createTrackingResult.Error);
        }

        var deliveryTracking = createTrackingResult.Value;

        // 9. Add delivery tracking details
        var trackingDetails = orderDetails.Select(detail =>
            DeliveryTrackingDetail.CreateProduct(
                deliveryTracking.Id,
                detail.VariantId ?? detail.ProductId,
                detail.Quantity)
        ).ToList();

        var addDetailsResult = deliveryTracking.AddDetails(trackingDetails);
        if (addDetailsResult.IsFailure)
        {
            return Result.Failure<Guid>(addDetailsResult.Error);
        }

        // 10. Persist to database
        _deliveryTrackingRepository.Add(deliveryTracking);

        return Result.Success(deliveryTracking.Id.Value);
    }

    /// <summary>
    /// Parse GHN response to extract delivery order code and expected delivery date
    /// </summary>
    private (string deliveryOrderCode, DateTime expectedDeliveryDate) ParseGhnResponse(object ghnResponse)
    {
        try
        {
            var jsonElement = JsonSerializer.Serialize(ghnResponse);
            var jsonDocument = JsonDocument.Parse(jsonElement);
            var root = jsonDocument.RootElement;

            // Extract data object
            if (!root.TryGetProperty("data", out var dataElement))
            {
                throw new InvalidOperationException("Missing 'data' property in GHN response");
            }

            // Extract order_code
            if (!dataElement.TryGetProperty("order_code", out var orderCodeElement))
            {
                throw new InvalidOperationException("Missing 'order_code' in GHN data");
            }

            // Extract expected_delivery_time
            if (!dataElement.TryGetProperty("expected_delivery_time", out var expectedDeliveryElement))
            {
                throw new InvalidOperationException("Missing 'expected_delivery_time' in GHN data");
            }

            var orderCode = orderCodeElement.GetString() ?? string.Empty;
            if (!DateTime.TryParse(expectedDeliveryElement.GetString(), out var expectedDeliveryTime))
            {
                throw new InvalidOperationException("Invalid 'expected_delivery_time' format");
            }

            return (orderCode, expectedDeliveryTime);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to parse GHN response: {ex.Message}", ex);
        }
    }
}
