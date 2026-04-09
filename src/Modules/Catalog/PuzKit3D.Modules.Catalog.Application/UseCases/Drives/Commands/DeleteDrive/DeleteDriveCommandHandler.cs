using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.DeleteDrive;

internal sealed class DeleteDriveCommandHandler : ICommandHandler<DeleteDriveCommand>
{
    private readonly IDriveRepository _driveRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteDriveCommandHandler(
        IDriveRepository driveRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _driveRepository = driveRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteDriveCommand request, CancellationToken cancellationToken)
    {
        var driveId = DriveId.From(request.Id);
        var drive = await _driveRepository.GetByIdAsync(driveId, cancellationToken);

        if (drive is null)
        {
            return Result.Failure(Error.NotFound("Drive.NotFound", $"Drive with ID {request.Id} not found"));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _driveRepository.Delete(drive);
            return Result.Success();
        }, cancellationToken);
    }
}
