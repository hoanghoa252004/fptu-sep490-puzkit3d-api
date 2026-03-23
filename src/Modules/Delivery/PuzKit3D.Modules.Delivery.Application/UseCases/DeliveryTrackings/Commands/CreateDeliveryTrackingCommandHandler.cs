using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Application.UnitOfWork;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Application.Data;
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
    private readonly ISupportTicketReplicaRepository _supportTicketReplicaRepository;
    private readonly IPartReplicaRepository _partReplicaRepository;
    private readonly IUserReplicaRepository _userReplicaRepository;
    private readonly IDeliveryUnitOfWork _unitOfWork;

    public CreateDeliveryTrackingCommandHandler(
        IDeliveryTrackingRepository deliveryTrackingRepository,
        IOrderReplicaRepository orderReplicaRepository,
        IDeliveryService deliveryService,
        IOrderDetailReplicaRepository orderDetailReplicaRepository,
        ISupportTicketReplicaRepository supportTicketReplicaRepository,
        IPartReplicaRepository partReplicaRepository,
        IUserReplicaRepository userReplicaRepository,
        IDeliveryUnitOfWork unitOfWork)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _deliveryService = deliveryService;
        _orderReplicaRepository = orderReplicaRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
        _supportTicketReplicaRepository = supportTicketReplicaRepository;
        _partReplicaRepository = partReplicaRepository;
        _userReplicaRepository = userReplicaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateDeliveryTrackingCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
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

            // 2b. If support type, verify all previous trackings are delivered
            if (trackingType == DeliveryTrackingType.Support)
            {
                var hasUndeliveredTracking = existingTrackings.Any(t => t.Status != DeliveryTrackingStatus.Delivered);
                if (hasUndeliveredTracking)
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.CannotCreateSupport",
                            "Cannot create support delivery tracking while previous deliveries are not yet delivered"));
                }
            }

            // 3. Validate SupportTicketId based on tracking type
            SupportTicketReplica? supportTicket = null;
            if (trackingType == DeliveryTrackingType.Support)
            {
                // For Support type, SupportTicketId is required
                if (!request.SupportTicketId.HasValue || request.SupportTicketId == Guid.Empty)
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.SupportTicketRequired",
                            "Support ticket is required for support delivery tracking"));
                }

                // Verify support ticket exists
                var ticketResult = await _supportTicketReplicaRepository.GetByIdAsync(request.SupportTicketId.Value, cancellationToken);
                if (ticketResult.IsFailure)
                {
                    return Result.Failure<Guid>(ticketResult.Error);
                }

                supportTicket = ticketResult.Value;

                // Check if support ticket status is Processing
                if (!supportTicket.Status.Equals("Processing", StringComparison.OrdinalIgnoreCase))
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.SupportTicketNotProcessing",
                            "Delivery tracking can only be created for support tickets with Processing status"));
                }
            }
            else if (request.SupportTicketId.HasValue && request.SupportTicketId != Guid.Empty)
            {
                // For Original type, SupportTicketId should not be provided
                return Result.Failure<Guid>(
                    Error.Validation("Delivery.SupportTicketNotAllowed",
                        "Support ticket should not be provided for original delivery tracking"));
            }

            // 4. Get order details
            var orderDetails = await _orderDetailReplicaRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            if (!orderDetails.Any())
            {
                return Result.Failure<Guid>(OrderReplicaError.OrderDetailNotFound(request.OrderId));
            }

            // 4b. Get user information
            var userResult = await _userReplicaRepository.GetByIdAsync(order.CustomerId, cancellationToken);
            if (userResult.IsFailure)
            {
                return Result.Failure<Guid>(userResult.Error);
            }

            var user = userResult.Value;

            // 5. Build shipping items
            List<ShippingOrderItem> items = new();

            if (trackingType == DeliveryTrackingType.Original)
            {
                // 1b. Check if order status is Processing
                if (!order.Status.Equals("Processing", StringComparison.OrdinalIgnoreCase))
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.OrderNotProcessing",
                            "Delivery tracking can only be created for orders with Processing status"));
                }

                // For Original delivery, use order details directly (products)
                items = orderDetails.Select(detail => new ShippingOrderItem
                {
                    Name = $"Product {detail.ProductId}",
                    Code = (detail.VariantId ?? detail.ProductId).ToString(),
                    Quantity = detail.Quantity,
                    Price = 0
                }).ToList();
            }
            else if (trackingType == DeliveryTrackingType.Support && supportTicket != null)
            {
                // For Support delivery, get details from support ticket
                var ticketDetailsResult = await _supportTicketReplicaRepository.GetDetailsByTicketIdAsync(supportTicket.Id, cancellationToken);
                if (ticketDetailsResult.IsFailure)
                {
                    return Result.Failure<Guid>(ticketDetailsResult.Error);
                }

                var ticketDetails = ticketDetailsResult.Value;

                // If support ticket type is ReplacePart, build items from parts
                if (supportTicket.Type == "ReplacePart")
                {
                    foreach (var detail in ticketDetails)
                    {
                        if (detail.PartId.HasValue)
                        {
                            // Get part information
                            var partResult = await _partReplicaRepository.GetByIdAsync(detail.PartId.Value, cancellationToken);
                            if (partResult.IsFailure)
                            {
                                return Result.Failure<Guid>(partResult.Error);
                            }

                            var part = partResult.Value;
                            var partDisplayName = $"{part.Name} - {part.PartType}";

                            items.Add(new ShippingOrderItem
                            {
                                Name = partDisplayName,
                                Code = part.Code,
                                Quantity = detail.Quantity,
                                Price = 0
                            });
                        }
                    }
                }
                else
                {
                    // For other support types, use order details
                    items = orderDetails.Select(detail => new ShippingOrderItem
                    {
                        Name = $"Product {detail.ProductId}",
                        Code = (detail.VariantId ?? detail.ProductId).ToString(),
                        Quantity = detail.Quantity,
                        Price = 0
                    }).ToList();
                }
            }

            // 6. Create shipping order request
            var orderCode = "";
            if(supportTicket != null)
            {
                orderCode = $"{order.Code}-{supportTicket.Code}";
            }
            else
            {
                orderCode = $"{order.Code}";
            }
            var shippingRequest = new CreateShippingOrderRequest
                {
                    ToName = user.FullName,
                    ToPhone = user.PhoneNumber,
                    ToAddress = user.StreetAddress!,
                    ToWardName = user.Ward!,
                    ToDistrictName = user.District!,
                    ToProvinceName = user.Province!,
                    OrderCode = Guid.NewGuid().ToString(),
                    RequiredNote = "CHOXEMHANGKHONGTHU",
                    Note = "Welcome to Puzkit3D",
                    Items = items,
                    Content = "Puzzle 3D Product",
                    CodAmount = 0
                };

            // 7. Create shipping order with delivery service
            var ghnResult = await _deliveryService.CreateShippingOrderAsync(shippingRequest, cancellationToken);
            if (ghnResult.IsFailure)
            {
                return Result.Failure<Guid>(ghnResult.Error);
            }

            // 8. Parse GHN response to extract delivery order code and expected delivery date
            var (deliveryOrderCode, expectedDeliveryDate) = ParseGhnResponse(ghnResult.Value);

            // 9. Create DeliveryTracking entity
            var createTrackingResult = DeliveryTracking.Create(
                order.Id,
                deliveryOrderCode,
                expectedDeliveryDate,
                trackingType,
                null,
                supportTicket?.Id);

            if (createTrackingResult.IsFailure)
            {
                return Result.Failure<Guid>(createTrackingResult.Error);
            }

            var deliveryTracking = createTrackingResult.Value;

            // 10. Add delivery tracking details
            List<DeliveryTrackingDetail> trackingDetails = new();

            if (trackingType == DeliveryTrackingType.Original)
            {
                // For Original delivery, use products from order details
                trackingDetails = orderDetails.Select(detail =>
                    DeliveryTrackingDetail.CreateProduct(
                        deliveryTracking.Id,
                        detail.VariantId ?? detail.ProductId,
                        detail.Quantity)
                ).ToList();
            }
            else if (trackingType == DeliveryTrackingType.Support && supportTicket?.Type == "ReplacePart")
            {
                // For Support delivery with ReplacePart, add parts
                var ticketDetailsResult = await _supportTicketReplicaRepository.GetDetailsByTicketIdAsync(supportTicket.Id, cancellationToken);
                if (ticketDetailsResult.IsSuccess)
                {
                    foreach (var detail in ticketDetailsResult.Value)
                    {
                        if (detail.PartId.HasValue)
                        {
                            trackingDetails.Add(
                                DeliveryTrackingDetail.CreatePart(
                                    deliveryTracking.Id,
                                    detail.PartId.Value,
                                    detail.Quantity));
                        }
                    }
                }
            }
            else
            {
                // For other support types, use products
                trackingDetails = orderDetails.Select(detail =>
                    DeliveryTrackingDetail.CreateProduct(
                        deliveryTracking.Id,
                        detail.VariantId ?? detail.ProductId,
                        detail.Quantity)
                ).ToList();
            }

            if (trackingDetails.Any())
            {
                var addDetailsResult = deliveryTracking.AddDetails(trackingDetails);
                if (addDetailsResult.IsFailure)
                {
                    return Result.Failure<Guid>(addDetailsResult.Error);
                }
            }

            // 11. Persist to database
            _deliveryTrackingRepository.Add(deliveryTracking);

            return Result.Success(deliveryTracking.Id.Value);
        });
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

