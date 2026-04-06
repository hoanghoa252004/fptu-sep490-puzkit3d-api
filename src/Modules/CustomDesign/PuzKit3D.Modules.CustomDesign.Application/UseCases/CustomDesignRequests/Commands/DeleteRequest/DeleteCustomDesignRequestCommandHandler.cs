using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.DeleteRequest;

internal sealed class DeleteCustomDesignRequestCommandHandler : ICommandHandler<DeleteCustomDesignRequestCommand>
{
    private readonly ICustomDesignRequestRepository _repository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;

    public DeleteCustomDesignRequestCommandHandler(
        ICustomDesignRequestRepository repository,
        ICustomDesignUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteCustomDesignRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Check if request exists
        var existingRequestResult = await _repository.GetByIdAsync(
            CustomDesignRequestId.From(request.Id),
            cancellationToken);

        if (existingRequestResult.IsFailure)
            return Result.Failure(existingRequestResult.Error);

        var existingRequest = existingRequestResult.Value;

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _repository.Delete(existingRequest);
            return Result.Success();
        });
    }
}
