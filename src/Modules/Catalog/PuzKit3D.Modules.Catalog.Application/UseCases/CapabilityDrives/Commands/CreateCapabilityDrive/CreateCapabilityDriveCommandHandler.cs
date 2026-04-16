using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.CreateCapabilityDrive;

internal sealed class CreateCapabilityDriveCommandHandler
    : ICommandTHandler<CreateCapabilityDriveCommand, object>
{
    private readonly ICapabilityDriveRepository _repository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateCapabilityDriveCommandHandler(
        ICapabilityDriveRepository repository,
        ICapabilityRepository capabilityRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<object>> Handle(
        CreateCapabilityDriveCommand request,
        CancellationToken cancellationToken)
    {
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var driveId = DriveId.From(request.DriveId);

        // Verify capability exists
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);
        if (capability is null)
        {
            return Result.Failure<object>(CapabilityError.NotFound(request.CapabilityId));
        }

        // Check if combination already exists
        var existing = await _repository.GetByIdAsync(capabilityId, driveId, cancellationToken);
        if (existing is not null)
        {
            return Result.Failure<object>(
                CapabilityDriveError.AlreadyExists(capabilityId, driveId));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var item = CapabilityDrive.Create(capabilityId, driveId);

            if (item.IsFailure)
            {
                return Result.Failure<object>(item.Error);
            }

            _repository.Add(item.Value);

            return Result.Success((object)new { capabilityId = request.CapabilityId, driveId = request.DriveId });
        }, cancellationToken);
    }
}
