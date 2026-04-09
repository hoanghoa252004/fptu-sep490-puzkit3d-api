using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.DeleteCapabilityDrive;

internal sealed class DeleteCapabilityDriveCommandHandler : ICommandHandler<DeleteCapabilityDriveCommand>
{
    private readonly ICapabilityDriveRepository _capabilityDriveRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteCapabilityDriveCommandHandler(
        ICapabilityDriveRepository capabilityDriveRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _capabilityDriveRepository = capabilityDriveRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCapabilityDriveCommand request, CancellationToken cancellationToken)
    {
        var capabilityDriveId = CapabilityDriveId.From(request.Id);
        var capabilityDrive = await _capabilityDriveRepository.GetByIdAsync(capabilityDriveId, cancellationToken);

        if (capabilityDrive is null)
        {
            return Result.Failure(
                Error.NotFound("CapabilityDrive.NotFound", $"CapabilityDrive with ID {request.Id} not found"));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _capabilityDriveRepository.Delete(capabilityDrive);
            return Result.Success();
        }, cancellationToken);
    }
}
