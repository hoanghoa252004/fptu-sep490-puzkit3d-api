using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries;

public sealed record GetDeliveryTrackingByIdQuery(Guid DeliveryTrackingId) : IQuery<DeliveryTrackingDto>;

