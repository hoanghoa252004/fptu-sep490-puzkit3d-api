using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.DeleteCapabilityDrive;

internal sealed class DeleteCapabilityDriveCommandHandler : ICommandHandler<DeleteCapabilityDriveCommand>
{
    private readonly ICapabilityDriveRepository _repository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteCapabilityDriveCommandHandler(
        ICapabilityDriveRepository repository,
        ICapabilityRepository capabilityRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteCapabilityDriveCommand request,
        CancellationToken cancellationToken)
    {
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
            return Result.Failure(CapabilityError.NotFound(request.CapabilityId));

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var driveId = DriveId.From(request.DriveId);
            var item = await _repository.GetByIdAsync(capabilityId, driveId, cancellationToken);

            if (item is null)
                return Result.Failure(CapabilityDriveError.NotFound(request.CapabilityId, request.DriveId));

            _repository.Delete(item);
            return Result.Success();
        });
    }
}
