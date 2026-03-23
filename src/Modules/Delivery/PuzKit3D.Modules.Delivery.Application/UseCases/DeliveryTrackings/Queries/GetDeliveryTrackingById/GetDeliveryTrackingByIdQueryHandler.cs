using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingById;

internal sealed class GetDeliveryTrackingByIdQueryHandler : IQueryHandler<GetDeliveryTrackingByIdQuery, DeliveryTrackingDto>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;

    public GetDeliveryTrackingByIdQueryHandler(IDeliveryTrackingRepository deliveryTrackingRepository)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
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

        var dto = MapToDto(tracking);
        return Result.Success(dto);
    }

    private static DeliveryTrackingDto MapToDto(DeliveryTracking tracking)
    {
        return new DeliveryTrackingDto(
            tracking.Id.Value,
            tracking.OrderId,
            tracking.SupportTicketId,
            tracking.DeliveryOrderCode,
            tracking.Status.ToString(),
            tracking.Type.ToString(),
            tracking.Note,
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

