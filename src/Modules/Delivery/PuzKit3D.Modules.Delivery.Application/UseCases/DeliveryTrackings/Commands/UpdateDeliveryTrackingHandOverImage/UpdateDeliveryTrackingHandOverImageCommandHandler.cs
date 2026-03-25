using MediatR;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.UnitOfWork;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands.UpdateDeliveryTrackingHandOverImage;

internal sealed class UpdateDeliveryTrackingHandOverImageCommandHandler
    : ICommandHandler<UpdateDeliveryTrackingHandOverImageCommand>
{
    private readonly IDeliveryTrackingRepository _repository;
    private readonly IDeliveryUnitOfWork _unitOfWork;

    public UpdateDeliveryTrackingHandOverImageCommandHandler(
        IDeliveryTrackingRepository repository,
        IDeliveryUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateDeliveryTrackingHandOverImageCommand request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Get delivery tracking
            var tracking = await _repository.GetByIdAsync(DeliveryTrackingId.From(request.DeliveryTrackingId), cancellationToken);
            if (tracking is null)
            {
                return Result.Failure<DeliveryTrackingDto>(
                    Error.NotFound("DeliveryTracking.NotFound",
                        $"Delivery tracking with ID {request.DeliveryTrackingId} not found"));
            }

            // Update the hand over image URL
            var updateResult = tracking.UpdateHandOverImageUrl(request.ImageUrl);
            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            // Update repository
            _repository.Update(tracking);

            return Result.Success();
        }, cancellationToken);
    }
}
