using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetCapabilityDrivesByCapabilityId;

internal sealed class GetCapabilityDrivesByCapabilityIdQueryHandler : IQueryHandler<GetCapabilityDrivesByCapabilityIdQuery, object>
{
    private readonly ICapabilityDriveRepository _repository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IDriveRepository _driveRepository;
    private readonly ICurrentUser _currentUser;

    public GetCapabilityDrivesByCapabilityIdQueryHandler(
        ICapabilityDriveRepository repository,
        ICapabilityRepository capabilityRepository,
        IDriveRepository driveRepository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _capabilityRepository = capabilityRepository;
        _driveRepository = driveRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetCapabilityDrivesByCapabilityIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Verify capability exists
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<object>(
                CapabilityError.NotFound(request.CapabilityId));
        }

        // For non-staff/manager users, only return if capability is active
        if (!isStaffOrManager && !capability.IsActive)
        {
            return Result.Failure<object>(
                CapabilityError.NotFound(request.CapabilityId));
        }

        // Get capability drives for this capability
        var items = await _repository.FindAsync(
            cd => cd.CapabilityId == capabilityId,
            cancellationToken);

        var responses = new List<GetCapabilityDriveResponseDto>();

        foreach (var cd in items)
        {
            var drive = await _driveRepository.GetByIdAsync(cd.DriveId, cancellationToken);

            if (drive is null)
                continue;

            responses.Add(new GetCapabilityDriveResponseDto(
                cd.CapabilityId.Value,
                cd.DriveId.Value,
                new(
                    drive.Id.Value,
                    drive.Name,
                    drive.Description,
                    drive.MinVolume,
                    drive.QuantityInStock,
                    drive.IsActive)));
        }

        return Result.Success((object)responses);
    }
}
