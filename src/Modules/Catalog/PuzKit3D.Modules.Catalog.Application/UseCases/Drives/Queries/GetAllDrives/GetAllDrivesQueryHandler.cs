using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetAllDrives;

internal sealed class GetAllDrivesQueryHandler : IQueryHandler<GetAllDrivesQuery, PagedResult<object>>
{
    private readonly IDriveRepository _driveRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllDrivesQueryHandler(IDriveRepository driveRepository, ICurrentUser currentUser)
    {
        _driveRepository = driveRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(GetAllDrivesQuery request, CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get all drives with filtering and sorting from repository
        var allDrives = await _driveRepository.GetAllAsync(
            isStaffOrManager,
            request.SearchTerm,
            request.Ascending,
            cancellationToken);

        // Apply pagination
        var totalCount = allDrives.Count();
        var drives = allDrives
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var driveDtos = drives.Select(d => (object)new GetDriveDetailedResponseDto(
            d.Id.Value,
            d.Name,
            d.Description,
            d.MinVolume,
            d.QuantityInStock,
            d.IsActive)).ToList();

        var pagedResult = new PagedResult<object>(
            driveDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
