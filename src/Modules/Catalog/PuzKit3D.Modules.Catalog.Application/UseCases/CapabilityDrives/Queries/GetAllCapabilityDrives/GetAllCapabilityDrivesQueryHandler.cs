using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetAllCapabilityDrives;

internal sealed class GetAllCapabilityDrivesQueryHandler : IQueryHandler<GetAllCapabilityDrivesQuery, IEnumerable<GetAllCapabilityDrivesResponseDto>>
{
    private readonly ICapabilityDriveRepository _capabilityDriveRepository;

    public GetAllCapabilityDrivesQueryHandler(ICapabilityDriveRepository capabilityDriveRepository)
    {
        _capabilityDriveRepository = capabilityDriveRepository;
    }

    public async Task<ResultT<IEnumerable<GetAllCapabilityDrivesResponseDto>>> Handle(GetAllCapabilityDrivesQuery request, CancellationToken cancellationToken)
    {
        var capabilityDrives = await _capabilityDriveRepository.GetAllAsync(cancellationToken);

        var response = capabilityDrives
            .Select(cd => new GetAllCapabilityDrivesResponseDto(
                cd.Id.Value,
                cd.CapabilityId.Value,
                cd.DriveId.Value,
                cd.Quantity))
            .ToList();

        return Result.Success((IEnumerable<GetAllCapabilityDrivesResponseDto>)response);
    }
}
