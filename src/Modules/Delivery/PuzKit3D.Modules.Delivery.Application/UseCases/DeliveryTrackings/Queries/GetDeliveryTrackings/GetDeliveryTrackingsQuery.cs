using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingsByOrderId;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackings;

public sealed record GetDeliveryTrackingsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Status = null) : IQuery<PaginatedDeliveryTrackingDto>;

