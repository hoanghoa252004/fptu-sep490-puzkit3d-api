using MediatR;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.UpdatePartnerProductQuotationStatus;

internal sealed class UpdatePartnerProductQuotationStatusCommandHandler : ICommandTHandler<UpdatePartnerProductQuotationStatusCommand, Guid>
{
    private readonly IPartnerProductQuotationRepository _quotationRepository;
    private readonly IPartnerProductRequestRepository _requestRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdatePartnerProductQuotationStatusCommandHandler(
        IPartnerProductQuotationRepository quotationRepository,
        IPartnerProductRequestRepository requestRepository,
        IPartnerUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _quotationRepository = quotationRepository;
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdatePartnerProductQuotationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            // Load quotation
            var existingQuotation = await _quotationRepository.GetByIdAsync(
                PartnerProductQuotationId.From(request.QuotationId),
                cancellationToken);

            if (existingQuotation == null)
            {
                return Result.Failure<Guid>(PartnerProductQuotationError.NotFound(request.QuotationId));
            }

            // Check permission
            if (!CanUpdateRequestStatus(_currentUser, request.NewStatus))
            {
                return Result.Failure<Guid>(PartnerProductQuotationError.PermissionDenied());
            }

            // Update quotation status
            var updateResult = existingQuotation.UpdateStatus(request.NewStatus, request.Note);
            if (updateResult.IsFailure)
            {
                return Result.Failure<Guid>(updateResult.Error);
            }

            // Load associated request and sync status
            var associatedRequest = await _requestRepository.GetByIdAsync(
                existingQuotation.PartnerProductRequestId,
                cancellationToken);

            if (associatedRequest != null)
            {
                // Map quotation status to request status
                var requestStatus = MapQuotationStatusToRequestStatus(request.NewStatus);
                var requestUpdateResult = associatedRequest.UpdateStatus(false, requestStatus, request.Note);
                
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

    // Maps quotation status changes to corresponding request status changes.
    private static PartnerProductRequestStatus MapQuotationStatusToRequestStatus(
        PartnerProductQuotationStatus quotationStatus)
    {
        return quotationStatus switch
        {
            PartnerProductQuotationStatus.Quoted => PartnerProductRequestStatus.Quoted,
            PartnerProductQuotationStatus.Accepted => PartnerProductRequestStatus.Accepted,
            PartnerProductQuotationStatus.RejectedByCustomer => PartnerProductRequestStatus.RejectedByCustomer,
            PartnerProductQuotationStatus.CancelledByStaff => PartnerProductRequestStatus.CancelledByStaff,
            PartnerProductQuotationStatus.CancelledByCustomer => PartnerProductRequestStatus.CancelledByCustomer,
            _ => throw new InvalidOperationException($"Unknown quotation status: {quotationStatus}")
        };
    }

    private bool CanUpdateRequestStatus(ICurrentUser user, PartnerProductQuotationStatus status)
    {
        return user.Roles.Any(role =>
            RolePermissions.ContainsKey(role) &&
            RolePermissions[role].Contains(status));
    }

    private static readonly Dictionary<string, HashSet<PartnerProductQuotationStatus>> RolePermissions = new()
    {
        {
            Roles.Customer, new HashSet<PartnerProductQuotationStatus>
            {
                PartnerProductQuotationStatus.Accepted,
                PartnerProductQuotationStatus.CancelledByCustomer,
                PartnerProductQuotationStatus.RejectedByCustomer
            }
        },
        {
            Roles.Staff, new HashSet<PartnerProductQuotationStatus>
            {
                PartnerProductQuotationStatus.CancelledByStaff
            }
        },
        {
            Roles.BusinessManager, new HashSet<PartnerProductQuotationStatus>
            {
                PartnerProductQuotationStatus.CancelledByStaff
            }
        }
    };
}
