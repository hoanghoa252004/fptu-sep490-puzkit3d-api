using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.UpdatePartnerProductQuotationStatus;

internal sealed class UpdatePartnerProductQuotationStatusCommandHandler : ICommandTHandler<UpdatePartnerProductQuotationStatusCommand, Guid>
{
    private readonly IPartnerProductQuotationRepository _quotationRepository;
    private readonly IPartnerProductRequestRepository _requestRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public UpdatePartnerProductQuotationStatusCommandHandler(
        IPartnerProductQuotationRepository quotationRepository,
        IPartnerProductRequestRepository requestRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _quotationRepository = quotationRepository;
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdatePartnerProductQuotationStatusCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            // Load quotation
            var existingQuotation = await _quotationRepository.GetByIdAsync(
                PartnerProductQuotationId.From(command.QuotationId),
                cancellationToken);

            if (existingQuotation == null)
            {
                return Result.Failure<Guid>(PartnerProductQuotationError.NotFound(command.QuotationId));
            }

            // Validate status transition
            var currentStatus = existingQuotation.Status;
            if (!PartnerProductQuotationStatusTransition.IsValidTransition(currentStatus, command.NewStatus))
            {
                return Result.Failure<Guid>(
                    PartnerProductQuotationError.InvalidStatusTransition(currentStatus, command.NewStatus));
            }

            // Update quotation status
            var updateResult = existingQuotation.UpdateStatus(command.NewStatus, command.Note);
            if (updateResult.IsFailure)
            {
                return Result.Failure<Guid>(updateResult.Error);
            }

            // Load associated request and sync status
            var associatedRequest = await _requestRepository.GetDetailByIdAsync(
                existingQuotation.PartnerProductRequestId,
                cancellationToken);

            if (associatedRequest != null)
            {
                // Map quotation status to request status
                var requestStatus = MapQuotationStatusToRequestStatus(command.NewStatus);
                var requestUpdateResult = associatedRequest.UpdateStatus(requestStatus, command.Note);
                
                if (requestUpdateResult.IsFailure)
                {
                    return Result.Failure<Guid>(requestUpdateResult.Error);
                }

                _requestRepository.Update(associatedRequest);
            }

            _quotationRepository.Update(existingQuotation);

            return Result.Success(existingQuotation.Id.Value);
        }, cancellationToken);

        return result;
    }

    /// <summary>
    /// Maps quotation status changes to corresponding request status changes.
    /// </summary>
    private static PartnerProductRequestStatus MapQuotationStatusToRequestStatus(
        PartnerProductQuotationStatus quotationStatus)
    {
        return quotationStatus switch
        {
            PartnerProductQuotationStatus.Pending => PartnerProductRequestStatus.Quoted,
            PartnerProductQuotationStatus.Quoted => PartnerProductRequestStatus.Quoted,
            PartnerProductQuotationStatus.Accepted => PartnerProductRequestStatus.Accepted,
            PartnerProductQuotationStatus.RejectedByCustomer => PartnerProductRequestStatus.RejectedByCustomer,
            PartnerProductQuotationStatus.CancelledByStaff => PartnerProductRequestStatus.CancelledByStaff,
            PartnerProductQuotationStatus.CancelledByCustomer => PartnerProductRequestStatus.CancelledByCustomer,
            _ => throw new InvalidOperationException($"Unknown quotation status: {quotationStatus}")
        };
    }
}
