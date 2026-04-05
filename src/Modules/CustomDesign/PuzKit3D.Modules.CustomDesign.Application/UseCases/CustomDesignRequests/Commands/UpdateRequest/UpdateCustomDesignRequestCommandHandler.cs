using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.UpdateRequest;

internal sealed class UpdateCustomDesignRequestCommandHandler : ICommandHandler<UpdateCustomDesignRequestCommand>
{
    private readonly ICustomDesignRequestRepository _repository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateCustomDesignRequestCommandHandler(
        ICustomDesignRequestRepository repository,
        ICustomDesignUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        UpdateCustomDesignRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Check if request exists
        var existingRequestResult = await _repository.GetByIdAsync(
            CustomDesignRequestId.From(request.Id),
            cancellationToken);

        if (existingRequestResult.IsFailure)
            return Result.Failure(existingRequestResult.Error);

        var existingRequest = existingRequestResult.Value;

        // Parse status if provided
        CustomDesignRequestStatus? statusEnum = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<CustomDesignRequestStatus>(request.Status, true, out var parsed))
                return Result.Failure(CustomDesignRequestError.InvalidType());
            statusEnum = parsed;
        }

        // Update request - validation happens in domain
        var isStaff = _currentUser.IsInRole(Roles.Staff);
        var updateResult = existingRequest.Update(
            request.DesiredLengthMm,
            request.DesiredWidthMm,
            request.DesiredHeightMm,
            request.Sketches,
            request.CustomerPrompt,
            request.DesiredDeliveryDate,
            request.DesiredQuantity,
            request.TargetBudget,
            statusEnum,
            request.Note,
            isStaff);

        if (!updateResult.IsSuccess)
            return updateResult;

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _repository.Update(existingRequest);
            return Result.Success();
        });
    }
}

