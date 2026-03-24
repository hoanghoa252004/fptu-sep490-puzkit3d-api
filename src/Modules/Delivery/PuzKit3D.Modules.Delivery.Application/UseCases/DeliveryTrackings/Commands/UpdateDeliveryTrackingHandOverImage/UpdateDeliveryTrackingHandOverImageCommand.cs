using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands.UpdateDeliveryTrackingHandOverImage;

public sealed record UpdateDeliveryTrackingHandOverImageCommand(
    Guid DeliveryTrackingId,
    string? ImageUrl) : ICommand;
