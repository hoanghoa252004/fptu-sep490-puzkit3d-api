using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.UpdateDrive;

internal sealed class UpdateDriveCommandHandler : ICommandHandler<UpdateDriveCommand>
{
    private readonly IDriveRepository _driveRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateDriveCommandHandler(
        IDriveRepository driveRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _driveRepository = driveRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateDriveCommand request, CancellationToken cancellationToken)
    {
        var driveId = DriveId.From(request.Id);
        var drive = await _driveRepository.GetByIdAsync(driveId, cancellationToken);

        if (drive is null)
        {
            return Result.Failure(DriveError.NotFound(request.Id));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            drive.Update(
                request.Name,
                request.Description,
                request.MinVolume,
                request.QuantityInStock,
                request.IsActive);

            _driveRepository.Update(drive);

            return Result.Success();
        }, cancellationToken);
    }
}
