using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingsByOrderId;

internal sealed class GetDeliveryTrackingsByOrderIdQueryHandler : IQueryHandler<GetDeliveryTrackingsByOrderIdQuery, PaginatedDeliveryTrackingDto>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly IMediaAssetService _mediaAssetService;

    public GetDeliveryTrackingsByOrderIdQueryHandler(IDeliveryTrackingRepository deliveryTrackingRepository, IMediaAssetService mediaAssetService)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _mediaAssetService = mediaAssetService;
    }

    public async Task<ResultT<PaginatedDeliveryTrackingDto>> Handle(
        GetDeliveryTrackingsByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        var trackings = await _deliveryTrackingRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);

        if (!trackings.Any())
        {
            return Result.Failure<PaginatedDeliveryTrackingDto>(
                Error.NotFound("DeliveryTracking.NotFound",
                    $"No delivery trackings found for order {request.OrderId}"));
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

        var dtos = paginatedTrackings.Select(x => MapToDto(x, _mediaAssetService)).ToList();

        var result = new PaginatedDeliveryTrackingDto(
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            dtos);

        return Result.Success(result);
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

