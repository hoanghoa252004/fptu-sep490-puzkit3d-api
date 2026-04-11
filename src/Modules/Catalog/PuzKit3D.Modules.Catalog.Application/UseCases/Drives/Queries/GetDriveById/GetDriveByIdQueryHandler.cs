using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetDriveById;

internal sealed class GetDriveByIdQueryHandler : IQueryHandler<GetDriveByIdQuery, object>
{
    private readonly IDriveRepository _driveRepository;
    private readonly ICurrentUser _currentUser;

    public GetDriveByIdQueryHandler(IDriveRepository driveRepository, ICurrentUser currentUser)
    {
        _driveRepository = driveRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(GetDriveByIdQuery request, CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        var driveId = DriveId.From(request.Id);
        var drive = await _driveRepository.GetByIdAsync(driveId, cancellationToken);

        if (drive is null)
        {
            return Result.Failure<object>(
                Error.NotFound("Drive.NotFound", $"Drive with ID {request.Id} not found"));
        }

        // For non-staff/manager users, only return active drives
        if (!isStaffOrManager && !drive.IsActive)
        {
            return Result.Failure<object>(
                Error.NotFound("Drive.NotFound", $"Drive with ID {request.Id} not found"));
        }

        object response = isStaffOrManager
            ? new GetDriveByIdDetailsResponseDto(
                drive.Id.Value,
                drive.Name,
                drive.Description,
                drive.MinVolume,
                drive.QuantityInStock,
                drive.IsActive,
                drive.CreatedAt,
                drive.UpdatedAt)
            : new GetDriveByIdResponseDto(
                drive.Id.Value,
                drive.Name,
                drive.Description,
                drive.MinVolume);

        return Result.Success(response);
    }
}
