using Microsoft.Extensions.Options;
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
using System.Xml.Linq;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;

public sealed class CreateDeliveryTrackingCommandHandler : ICommandTHandler<CreateDeliveryTrackingCommand, Guid>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IDeliveryService _deliveryService;
    private readonly IOrderDetailReplicaRepository _orderDetailReplicaRepository;
    private readonly ISupportTicketReplicaRepository _supportTicketReplicaRepository;
    private readonly IDriveReplicaRepository _driveReplicaRepository;
    private readonly IUserReplicaRepository _userReplicaRepository;
    private readonly IDeliveryUnitOfWork _unitOfWork;
    private readonly DeliveryApplicationSettings _settings;

    public CreateDeliveryTrackingCommandHandler(
        IDeliveryTrackingRepository deliveryTrackingRepository,
        IOrderReplicaRepository orderReplicaRepository,
        IDeliveryService deliveryService,
        IOrderDetailReplicaRepository orderDetailReplicaRepository,
        ISupportTicketReplicaRepository supportTicketReplicaRepository,
        IDriveReplicaRepository driveReplicaRepository,
        IUserReplicaRepository userReplicaRepository,
        IDeliveryUnitOfWork unitOfWork,
        IOptions<DeliveryApplicationSettings> options)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _deliveryService = deliveryService;
        _orderReplicaRepository = orderReplicaRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
        _supportTicketReplicaRepository = supportTicketReplicaRepository;
        _driveReplicaRepository = driveReplicaRepository;
        _userReplicaRepository = userReplicaRepository;
        _unitOfWork = unitOfWork;
        _settings = options.Value;
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

            // 2. Determine tracking type and validate conditions
            var existingTrackings = await _deliveryTrackingRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            DeliveryTrackingType trackingType;
            SupportTicketReplica? supportTicket = null;

            // Check if this is the first tracking (Original type)
            if (!existingTrackings.Any())
            {
                // Original type: Order must be in Processing status
                if (!order.Status.Equals("Processing", StringComparison.OrdinalIgnoreCase))
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.OrderNotProcessing",
                            "Delivery tracking can only be created for orders with Processing status"));
                }

                // SupportTicketId should not be provided for Original type
                if (request.SupportTicketId.HasValue && request.SupportTicketId != Guid.Empty)
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.SupportTicketNotAllowed",
                            "Support ticket should not be provided for original delivery tracking"));
                }

                trackingType = DeliveryTrackingType.Original;
            }
            else
            {
                // For Return or Resend type, SupportTicketId is required
                if (!request.SupportTicketId.HasValue || request.SupportTicketId == Guid.Empty)
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.SupportTicketRequired",
                            "Support ticket is required for return or resend delivery tracking"));
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

                // Determine if this is Return or Resend type
                var lastOriginalTracking = existingTrackings.FirstOrDefault(t => t.Type == DeliveryTrackingType.Original);
                
                // Get trackings for this specific support ticket
                var trackingsForThisTicket = existingTrackings.Where(t => t.SupportTicketId == request.SupportTicketId.Value).ToList();
                var returnTrackingForThisTicket = trackingsForThisTicket.FirstOrDefault(t => t.Type == DeliveryTrackingType.Return);
                var resendTrackingForThisTicket = trackingsForThisTicket.FirstOrDefault(t => t.Type == DeliveryTrackingType.Resend);

                // Return type: Must have Original tracking that is Delivered, and SupportTicket type in (Return, Exchange, ReplaceDrive)
                if (lastOriginalTracking != null && lastOriginalTracking.Status == DeliveryTrackingStatus.Delivered)
                {
                    var validReturnTicketTypes = new[] { "Return", "Exchange", "ReplaceDrive" };
                    if (validReturnTicketTypes.Contains(supportTicket.Type, StringComparer.OrdinalIgnoreCase))
                    {
                        // For Return type support ticket, only 1 Return delivery is allowed
                        if (supportTicket.Type.Equals("Return", StringComparison.OrdinalIgnoreCase))
                        {
                            if (returnTrackingForThisTicket != null)
                            {
                                return Result.Failure<Guid>(
                                    Error.Validation("Delivery.ReturnDeliveryAlreadyExists",
                                        "Return delivery tracking already exists for this support ticket"));
                            }
                            trackingType = DeliveryTrackingType.Return;
                        }
                        // For Exchange and ReplaceDrive types, can have Return then Resend
                        else if (supportTicket.Type.Equals("Exchange", StringComparison.OrdinalIgnoreCase) || 
                                 supportTicket.Type.Equals("ReplaceDrive", StringComparison.OrdinalIgnoreCase))
                        {
                            // If Return tracking doesn't exist yet, create it
                            if (returnTrackingForThisTicket == null)
                            {
                                trackingType = DeliveryTrackingType.Return;
                            }
                            // If Return tracking exists and is Delivered, create Resend
                            else if (returnTrackingForThisTicket.Status == DeliveryTrackingStatus.Delivered)
                            {
                                // Cannot create another tracking if Resend already exists
                                if (resendTrackingForThisTicket != null)
                                {
                                    return Result.Failure<Guid>(
                                        Error.Validation("Delivery.ResendDeliveryAlreadyExists",
                                            "Resend delivery tracking already exists for this support ticket"));
                                }
                                trackingType = DeliveryTrackingType.Resend;
                            }
                            else
                            {
                                return Result.Failure<Guid>(
                                    Error.Validation("Delivery.ReturnNotDelivered",
                                        "Return delivery must be delivered before creating resend delivery"));
                            }
                        }
                        else
                        {
                            return Result.Failure<Guid>(
                                Error.Validation("Delivery.InvalidSupportTicketType",
                                    "Invalid support ticket type for delivery tracking"));
                        }
                    }
                    else
                    {
                        return Result.Failure<Guid>(
                            Error.Validation("Delivery.InvalidTrackingSequence",
                                "Invalid delivery tracking sequence or support ticket type"));
                    }
                }
                else
                {
                    return Result.Failure<Guid>(
                        Error.Validation("Delivery.CannotCreateDeliveryTracking",
                            "Cannot create delivery tracking. Original tracking must be delivered first"));
                }
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

            // 5. Build shipping items and determine address (From/To) based on tracking type
            List<ShippingOrderItem> items = new();
            //string toName, toPhone, toAddress, toWardName, toDistrictName, toProvinceName;
            ToDto toDto = null!;
            FromDto fromDto = null!;
            if (trackingType == DeliveryTrackingType.Original)
            {
                // For Original delivery, use order details directly (products)
                items = orderDetails.Select(detail => new ShippingOrderItem
                {
                    Name = $"{detail.ProductName} - {detail.VariantName}",
                    Code = "[PRODUCT]",
                    Quantity = detail.Quantity,
                    Price = 0
                }).ToList();

                // Ship to customer
                fromDto = new FromDto(
                    _settings.MyShop.Name,
                    _settings.MyShop.Phone,
                    _settings.MyShop.Address,
                    _settings.MyShop.Ward,
                    _settings.MyShop.District,
                    _settings.MyShop.Province
                    );

                toDto = new ToDto(
                    user.FullName,
                    user.PhoneNumber,
                    user.StreetAddress,
                    user.Ward,
                    user.District,
                    user.Province
                    );
                //toName = user.FullName;
                //toPhone = user.PhoneNumber;
                //toAddress = user.StreetAddress!;
                //toWardName = user.Ward!;
                //toDistrictName = user.District!;
                //toProvinceName = user.Province!;
            }
            else if (trackingType == DeliveryTrackingType.Return && supportTicket != null)
            {
                // For Return delivery, get details from support ticket
                var ticketDetailsResult = await _supportTicketReplicaRepository.GetDetailsByTicketIdAsync(supportTicket.Id, cancellationToken);
                if (ticketDetailsResult.IsFailure)
                {
                    return Result.Failure<Guid>(ticketDetailsResult.Error);
                }

                var ticketDetails = ticketDetailsResult.Value;

                // If support ticket type is ReplaceDrive, build items from drives
                if (supportTicket.Type == "ReplaceDrive")
                {
                    foreach (var detail in ticketDetails)
                    {
                        if (detail.DriveId.HasValue)
                        {
                            // Get drive information
                            var drive = await _driveReplicaRepository.GetByIdAsync(detail.DriveId.Value, cancellationToken);

                            if (drive is null)
                            {
                                return Result.Failure<Guid>(DeliveryTrackingError.DriveNotFound(detail.DriveId.Value));
                            }

                            items.Add(new ShippingOrderItem
                            {
                                Name = drive.Name,
                                Code = "[DRIVE]",
                                Quantity = detail.Quantity,
                                Price = 0
                            });
                        }
                    }
                }
                else
                {
                    // For other return types (Exchange, Return), use order details
                    items = orderDetails.Select(detail => new ShippingOrderItem
                    {
                        Name = $"{detail.ProductName}",
                        Code = "[PRODUCT]",
                        Quantity = detail.Quantity,
                        Price = 0
                    }).ToList();
                }

                // Ship from customer back to shop
                fromDto = new FromDto(
                    user.FullName,
                    user.PhoneNumber,
                    user.StreetAddress,
                    user.Ward,
                    user.District,
                    user.Province
                    );

                toDto = new ToDto(
                    _settings.MyShop.Name,
                    _settings.MyShop.Phone,
                    _settings.MyShop.Address,
                    _settings.MyShop.Ward,
                    _settings.MyShop.District,
                    _settings.MyShop.Province
                    );
            }
            else if (trackingType == DeliveryTrackingType.Resend && supportTicket != null)
            {
                // For Resend delivery, use order details (products)
                items = orderDetails.Select(detail => new ShippingOrderItem
                {
                    Name = $"{detail.ProductName} - {detail.VariantName}",
                    Code = (detail.VariantId ?? detail.ProductId).ToString(),
                    Quantity = detail.Quantity,
                    Price = 0
                }).ToList();

                // Ship to customer again
                fromDto = new FromDto(
                    _settings.MyShop.Name,
                    _settings.MyShop.Phone,
                    _settings.MyShop.Address,
                    _settings.MyShop.Ward,
                    _settings.MyShop.District,
                    _settings.MyShop.Province
                    );

                toDto = new ToDto(
                    user.FullName,
                    user.PhoneNumber,
                    user.StreetAddress,
                    user.Ward,
                    user.District,
                    user.Province
                    );
            }
            else
            {
                return Result.Failure<Guid>(
                    Error.Validation("Delivery.InvalidTrackingType",
                        "Invalid delivery tracking type"));
            }

            // 6. Create shipping order request
            var orderCode = trackingType switch
            {
                DeliveryTrackingType.Original => order.Code,
                _ => $"{order.Code}-{supportTicket?.Code}"
            };

            var shippingRequest = new CreateShippingOrderRequest
            {
                ToName = toDto.FullName,
                ToPhone = toDto.PhoneNumber,
                ToAddress = toDto.StreetAddress,
                ToWardName = toDto.Ward,
                ToDistrictName = toDto.District,
                ToProvinceName = toDto.Province,
                OrderCode = Guid.NewGuid().ToString(),
                RequiredNote = "CHOXEMHANGKHONGTHU",
                Note = "PuzKit3D Shop",
                Items = items,
                Content = "Puzzle 3D Product",
                CodAmount = trackingType == DeliveryTrackingType.Original && !order.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase) ? (int)order.GrandTotalAmount : 0
            };

            // 7. Create shipping order with delivery service
            var ghnResult = await _deliveryService.CreateShippingOrderAsync(shippingRequest, fromDto, toDto, trackingType, cancellationToken);
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
            else if (trackingType == DeliveryTrackingType.Return && supportTicket?.Type == "ReplaceDrive")
            {
                // For Return delivery with ReplaceDrive, add drives
                var ticketDetailsResult = await _supportTicketReplicaRepository.GetDetailsByTicketIdAsync(supportTicket.Id, cancellationToken);
                if (ticketDetailsResult.IsSuccess)
                {
                    foreach (var detail in ticketDetailsResult.Value)
                    {
                        if (detail.DriveId.HasValue)
                        {
                            trackingDetails.Add(
                                DeliveryTrackingDetail.CreateDrive(
                                    deliveryTracking.Id,
                                    detail.DriveId.Value,
                                    detail.Quantity));
                        }
                    }
                }
            }
            else if (trackingType == DeliveryTrackingType.Return || trackingType == DeliveryTrackingType.Resend)
            {
                // For Return and Resend types, use products from order details
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

