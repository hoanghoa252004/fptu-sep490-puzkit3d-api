using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetAllDrives;

internal sealed class GetAllDrivesQueryHandler : IQueryHandler<GetAllDrivesQuery, IEnumerable<GetAllDrivesResponseDto>>
{
    private readonly IDriveRepository _driveRepository;

    public GetAllDrivesQueryHandler(IDriveRepository driveRepository)
    {
        _driveRepository = driveRepository;
    }

    public async Task<ResultT<IEnumerable<GetAllDrivesResponseDto>>> Handle(GetAllDrivesQuery request, CancellationToken cancellationToken)
    {
        var drives = await _driveRepository.GetAllAsync(cancellationToken);

        var response = drives
            .Select(d => new GetAllDrivesResponseDto(
                d.Id.Value,
                d.Name,
                d.Description,
                d.MinVolume,
                d.QuantityInStock,
                d.IsActive))
            .ToList();

        return Result.Success((IEnumerable<GetAllDrivesResponseDto>)response);
    }
}
