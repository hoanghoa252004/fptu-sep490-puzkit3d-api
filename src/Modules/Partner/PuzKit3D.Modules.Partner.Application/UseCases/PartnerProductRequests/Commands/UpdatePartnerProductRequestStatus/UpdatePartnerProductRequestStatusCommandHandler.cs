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
    private readonly IPartnerProductQuotationRepository _quotationRepository;

    public UpdatePartnerProductRequestStatusCommandHandler(
        IPartnerProductRequestRepository requestRepository,
        IPartnerUnitOfWork unitOfWork,
        IPartnerProductQuotationRepository quotationRepository)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _quotationRepository = quotationRepository;
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

            // Check existing quotation
            var hasQuotation = await _quotationRepository.ExistsByRequestIdAsync(
                existingRequest.Id,
                cancellationToken);

            // Update status
            var updateResult = existingRequest.UpdateStatus(hasQuotation, request.NewStatus, request.Note);
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
