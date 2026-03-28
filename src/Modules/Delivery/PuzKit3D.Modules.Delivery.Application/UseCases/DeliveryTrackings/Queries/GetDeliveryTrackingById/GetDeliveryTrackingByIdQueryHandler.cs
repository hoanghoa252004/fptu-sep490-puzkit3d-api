using PuzKit3D.Contract.Delivery;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Application.UnitOfWork;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingById;

internal static class GhnStatusMapperForDeliveryTracking
{
    public static DeliveryTrackingStatus? MapGhnStatusToDeliveryTrackingStatus(string? ghnStatus)
    {
        if (string.IsNullOrWhiteSpace(ghnStatus))
            return null;

        return ghnStatus.ToLowerInvariant() switch
        {
            "picked" => DeliveryTrackingStatus.Picked,
            "delivering" => DeliveryTrackingStatus.Delivering,
            "delivered" => DeliveryTrackingStatus.Delivered,
            "return" => DeliveryTrackingStatus.Return,
            "returned" => DeliveryTrackingStatus.Returned,
            _ => null
        };
    }
}

internal sealed class GetDeliveryTrackingByIdQueryHandler : IQueryHandler<GetDeliveryTrackingByIdQuery, DeliveryTrackingDto>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly IDeliveryService _deliveryService;
    private readonly IDeliveryUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IMediaAssetService _mediaAssetService;

    public GetDeliveryTrackingByIdQueryHandler(
        IDeliveryTrackingRepository deliveryTrackingRepository,
        IDeliveryService deliveryService,
        IDeliveryUnitOfWork unitOfWork,
        IEventBus eventbus,
        IMediaAssetService mediaAssetService)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _deliveryService = deliveryService;
        _unitOfWork = unitOfWork;
        _eventBus = eventbus;
        _mediaAssetService = mediaAssetService;
    }

    public async Task<ResultT<DeliveryTrackingDto>> Handle(
        GetDeliveryTrackingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var trackingId = DeliveryTrackingId.From(request.DeliveryTrackingId);
        var tracking = await _deliveryTrackingRepository.GetByIdAsync(trackingId, cancellationToken);

        if (tracking is null)
        {
            return Result.Failure<DeliveryTrackingDto>(
                Error.NotFound("DeliveryTracking.NotFound", 
                    $"Delivery tracking with ID {request.DeliveryTrackingId} not found"));
        }

            // Sync status from GHN
            await SyncDeliveryTrackingStatusFromGhnAsync(tracking, cancellationToken);
            
            if(tracking.Status == DeliveryTrackingStatus.Delivered)
            {
                await _eventBus.PublishAsync(new OrderDeliveredIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, tracking.OrderId), cancellationToken);
            }
            else if(tracking.Status == DeliveryTrackingStatus.Returned)
            {
                await _eventBus.PublishAsync(new OrderReturnedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, tracking.OrderId), cancellationToken);
            }
            
            var dto = MapToDto(tracking, _mediaAssetService);
            return Result.Success(dto);
    }

    private static DeliveryTrackingDto MapToDto(DeliveryTracking tracking, IMediaAssetService _mediaAssetService)
    {
        var handOverImageUrl = string.IsNullOrWhiteSpace(tracking.HandOverImageUrl) 
            ? null 
            : _mediaAssetService.BuildAssetUrl(tracking.HandOverImageUrl);
        return new DeliveryTrackingDto(
            tracking.Id.Value,
            tracking.OrderId,
            tracking.SupportTicketId,
            tracking.DeliveryOrderCode,
            tracking.Status.ToString(),
            tracking.Type.ToString(),
            tracking.Note,
            handOverImageUrl,
            tracking.ExpectedDeliveryDate,
            tracking.DeliveredAt,
            tracking.CreatedAt,
            tracking.UpdatedAt,
            tracking.Details.Select(d => new DeliveryTrackingDetailDto(
                d.Id,
                d.Type.ToString(),
                d.ItemId,
                d.Quantity)).ToList());
    }

    private async Task SyncDeliveryTrackingStatusFromGhnAsync(DeliveryTracking tracking, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(tracking.DeliveryOrderCode))
            return;

        var result = await _deliveryService.GetShippingOrderDetailAsync(tracking.DeliveryOrderCode);

        if (result.IsFailure)
            return;

        try
        {
            // Serialize the response to JSON and parse it
            var json = JsonSerializer.Serialize(result.Value);
            using var doc = JsonDocument.Parse(json);
            
            // Navigate to data.status
            if (!doc.RootElement.TryGetProperty("data", out var dataProperty))
                return;

            if (!dataProperty.TryGetProperty("status", out var statusProperty))
                return;

            var ghnStatus = statusProperty.GetString();

            // Map GHN status to DeliveryTrackingStatus
            var mappedStatus = GhnStatusMapperForDeliveryTracking.MapGhnStatusToDeliveryTrackingStatus(ghnStatus);

            // Check if status needs to be updated
            if (mappedStatus.HasValue && mappedStatus.Value != tracking.Status)
            {
                // Update the tracking status based on the new status
                var updateResult = mappedStatus.Value switch
                {
                    DeliveryTrackingStatus.Picked => tracking.MarkAsPicked(),
                    DeliveryTrackingStatus.Delivering => tracking.MarkAsShipping(),
                    DeliveryTrackingStatus.Delivered => tracking.MarkAsDelivered(),
                    DeliveryTrackingStatus.Return => tracking.MarkAsReturn(),
                    DeliveryTrackingStatus.Returned => tracking.MarkAsReturned(),
                    _ => Result.Failure(Error.Validation("InvalidStatus", "Invalid delivery tracking status"))
                };

                if (updateResult.IsSuccess)
                {
                    // Update the tracking in the repository
                    _deliveryTrackingRepository.Update(tracking);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
        }
        catch
        {
            // Silently continue if parsing fails
            return;
        }
    }
}

