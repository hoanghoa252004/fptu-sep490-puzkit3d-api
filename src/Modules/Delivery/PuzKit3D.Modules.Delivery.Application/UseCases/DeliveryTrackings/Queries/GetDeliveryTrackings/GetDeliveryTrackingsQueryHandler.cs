using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingsByOrderId;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackings;

internal sealed class GetDeliveryTrackingsQueryHandler : IQueryHandler<GetDeliveryTrackingsQuery, PaginatedDeliveryTrackingDto>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;

    public GetDeliveryTrackingsQueryHandler(IDeliveryTrackingRepository deliveryTrackingRepository)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
    }

    public async Task<ResultT<PaginatedDeliveryTrackingDto>> Handle(
        GetDeliveryTrackingsQuery request,
        CancellationToken cancellationToken)
    {
        var trackings = await _deliveryTrackingRepository.GetAllAsync(cancellationToken);

        if (!trackings.Any())
        {
            return Result.Failure<PaginatedDeliveryTrackingDto>(
                Error.NotFound("DeliveryTracking.NotFound",
                    "No delivery trackings found"));
        }

        // Filter by status if provided
        var query = trackings.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<DeliveryTrackingStatus>(request.Status, ignoreCase: true, out var statusEnum))
            {
                query = query.Where(t => t.Status == statusEnum);
            }
        }

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        // Apply pagination
        var paginatedTrackings = query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paginatedTrackings.Select(MapToDto).ToList();

        var result = new PaginatedDeliveryTrackingDto(
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            dtos);

        return Result.Success(result);
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
            tracking.HandOverImageUrl,
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


