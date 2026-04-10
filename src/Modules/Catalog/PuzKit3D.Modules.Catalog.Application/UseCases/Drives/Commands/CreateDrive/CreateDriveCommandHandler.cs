using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.CreateDrive;

internal sealed class CreateDriveCommandHandler : ICommandTHandler<CreateDriveCommand, Guid>
{
    private readonly IDriveRepository _driveRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateDriveCommandHandler(
        IDriveRepository driveRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _driveRepository = driveRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateDriveCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var drive = Drive.Create(
                request.Name,
                request.Description,
                request.MinVolume,
                request.QuantityInStock,
                request.IsActive);

            if (drive.IsFailure)
            {
                return Result.Failure<Guid>(drive.Error);
            }

            _driveRepository.Add(drive.Value);

            return Result.Success(drive.Value.Id.Value);
        }, cancellationToken);
    }
}
