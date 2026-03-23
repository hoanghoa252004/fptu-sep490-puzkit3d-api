using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingsByOrderId;

public sealed record GetDeliveryTrackingsByOrderIdQuery(
    Guid OrderId,
    int PageNumber = 1,
    int PageSize = 10,
    string? Status = null) : IQuery<PaginatedDeliveryTrackingDto>;

public sealed record PaginatedDeliveryTrackingDto(
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    List<DeliveryTrackingDto> Data);

