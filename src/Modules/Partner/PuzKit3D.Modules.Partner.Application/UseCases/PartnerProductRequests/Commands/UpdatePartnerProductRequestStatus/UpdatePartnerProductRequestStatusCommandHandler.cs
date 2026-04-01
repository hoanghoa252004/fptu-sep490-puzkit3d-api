using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.UpdatePartnerProductRequestStatus;

internal sealed class UpdatePartnerProductRequestStatusCommandHandler : ICommandTHandler<UpdatePartnerProductRequestStatusCommand, Guid>
{
    private readonly IPartnerProductRequestRepository _requestRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public UpdatePartnerProductRequestStatusCommandHandler(
        IPartnerProductRequestRepository requestRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdatePartnerProductRequestStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            // Lấy request
            var existingRequest = await _requestRepository.GetDetailByIdAsync(
                PartnerProductRequestId.From(request.RequestId),
                cancellationToken);

            if (existingRequest == null)
            {
                return Result.Failure<Guid>(PartnerProductRequestError.NotFound(request.RequestId));
            }

            // Validate status transition
            var currentStatus = existingRequest.Status;
            if (!PartnerProductRequestStatusTransition.IsValidTransition(currentStatus, request.NewStatus))
            {
                return Result.Failure<Guid>(
                    PartnerProductRequestError.InvalidStatusTransition(currentStatus, request.NewStatus));
            }

            // Update status
            var updateResult = existingRequest.UpdateStatus(request.NewStatus, request.Note);
            if (updateResult.IsFailure)
            {
                return Result.Failure<Guid>(updateResult.Error);
            }

            _requestRepository.Update(existingRequest);

            return Result.Success(existingRequest.Id.Value);
        }, cancellationToken);

        return result;
    }
}
