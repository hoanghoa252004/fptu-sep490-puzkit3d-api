using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingBySupportTicketId;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingBySupportTicketId;

internal sealed class GetDeliveryTrackingBySupportTicketIdQueryHandler : IQueryHandler<GetDeliveryTrackingBySupportTicketIdQuery, DeliveryTrackingDto>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly ISupportTicketReplicaRepository _supportTicketReplicaRepository;
    private readonly IMediaAssetService _mediaAssetService;

    public GetDeliveryTrackingBySupportTicketIdQueryHandler(
        IDeliveryTrackingRepository deliveryTrackingRepository,
        ISupportTicketReplicaRepository supportTicketReplicaRepository,
        IMediaAssetService mediaAssetService)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _supportTicketReplicaRepository = supportTicketReplicaRepository;
        _mediaAssetService = mediaAssetService;
    }

    public async Task<ResultT<DeliveryTrackingDto>> Handle(
        GetDeliveryTrackingBySupportTicketIdQuery request,
        CancellationToken cancellationToken)
    {
        var supportTicketResult = await _supportTicketReplicaRepository.GetByIdAsync(request.SupportTicketId, cancellationToken);

        if (supportTicketResult.IsFailure)
        {
            return Result.Failure<DeliveryTrackingDto>(
                Error.NotFound("SupportTicket.NotFound",
                    $"Support ticket {request.SupportTicketId} not found"));
        }

        var trackings = await _deliveryTrackingRepository.GetBySupportTicketIdAsync(request.SupportTicketId, cancellationToken);

        if (!trackings.Any())
        {
            return Result.Failure<DeliveryTrackingDto>(
                Error.NotFound("DeliveryTracking.NotFound",
                    $"No delivery tracking found for support ticket {request.SupportTicketId}"));
        }

        var tracking = trackings.First();
        var dto = MapToDto(tracking, _mediaAssetService);

        return Result.Success(dto);
    }

    private static DeliveryTrackingDto MapToDto(DeliveryTracking tracking, IMediaAssetService mediaAssetService)
    {
        var handOverImageUrl = string.IsNullOrWhiteSpace(tracking.HandOverImageUrl)
            ? null
            : mediaAssetService.BuildAssetUrl(tracking.HandOverImageUrl);

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
}

