using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.CreateCapabilityDrive;

internal sealed class CreateCapabilityDriveCommandHandler : ICommandTHandler<CreateCapabilityDriveCommand, Guid>
{
    private readonly ICapabilityDriveRepository _capabilityDriveRepository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IDriveRepository _driveRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateCapabilityDriveCommandHandler(
        ICapabilityDriveRepository capabilityDriveRepository,
        ICapabilityRepository capabilityRepository,
        IDriveRepository driveRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _capabilityDriveRepository = capabilityDriveRepository;
        _capabilityRepository = capabilityRepository;
        _driveRepository = driveRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateCapabilityDriveCommand request, CancellationToken cancellationToken)
    {
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<Guid>(
                Error.NotFound("Capability.NotFound", $"Capability with ID {request.CapabilityId} not found"));
        }

        var driveId = DriveId.From(request.DriveId);
        var drive = await _driveRepository.GetByIdAsync(driveId, cancellationToken);

        if (drive is null)
        {
            return Result.Failure<Guid>(
                Error.NotFound("Drive.NotFound", $"Drive with ID {request.DriveId} not found"));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var capabilityDrive = CapabilityDrive.Create(capabilityId, driveId, request.Quantity);

            _capabilityDriveRepository.Add(capabilityDrive);

            return Result.Success(capabilityDrive.Id.Value);
        }, cancellationToken);
    }
}
