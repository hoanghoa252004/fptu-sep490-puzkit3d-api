using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingBySupportTicketId;

public sealed record GetDeliveryTrackingBySupportTicketIdQuery(Guid SupportTicketId) : IQuery<DeliveryTrackingDto>;
