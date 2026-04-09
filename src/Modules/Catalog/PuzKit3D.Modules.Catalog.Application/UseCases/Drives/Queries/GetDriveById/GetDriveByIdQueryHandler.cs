using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetDriveById;

internal sealed class GetDriveByIdQueryHandler : IQueryHandler<GetDriveByIdQuery, GetDriveByIdResponseDto>
{
    private readonly IDriveRepository _driveRepository;

    public GetDriveByIdQueryHandler(IDriveRepository driveRepository)
    {
        _driveRepository = driveRepository;
    }

    public async Task<ResultT<GetDriveByIdResponseDto>> Handle(GetDriveByIdQuery request, CancellationToken cancellationToken)
    {
        var driveId = DriveId.From(request.Id);
        var drive = await _driveRepository.GetByIdAsync(driveId, cancellationToken);

        if (drive is null)
        {
            return Result.Failure<GetDriveByIdResponseDto>(
                Error.NotFound("Drive.NotFound", $"Drive with ID {request.Id} not found"));
        }

        var response = new GetDriveByIdResponseDto(
            drive.Id.Value,
            drive.Name,
            drive.Description,
            drive.MinVolume,
            drive.QuantityInStock,
            drive.IsActive,
            drive.CreatedAt,
            drive.UpdatedAt);

        return Result.Success(response);
    }
}
