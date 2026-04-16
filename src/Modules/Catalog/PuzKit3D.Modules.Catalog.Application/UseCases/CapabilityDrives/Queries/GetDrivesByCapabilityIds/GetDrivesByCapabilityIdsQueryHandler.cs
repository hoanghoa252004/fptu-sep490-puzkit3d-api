using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetDrivesByCapabilityIds;

internal sealed class GetDrivesByCapabilityIdsQueryHandler
    : IQueryHandler<GetDrivesByCapabilityIdsQuery, List<GetDrivesByCapabilityIdsResponseDtos>>
{
    private readonly ICapabilityDriveRepository _repository;
    private readonly IDriveRepository _driveRepository;

    public GetDrivesByCapabilityIdsQueryHandler(
        ICapabilityDriveRepository repository,
        IDriveRepository driveRepository)
    {
        _repository = repository;
        _driveRepository = driveRepository;
    }

    public async Task<ResultT<List<GetDrivesByCapabilityIdsResponseDtos>>> Handle(
        GetDrivesByCapabilityIdsQuery request,
        CancellationToken cancellationToken)
    {
        if (!request.CapabilityIds.Any())
            return Result.Success(new List<GetDrivesByCapabilityIdsResponseDtos>());

        // Get all CapabilityDrives for these capabilities
        var capabilityIds = request.CapabilityIds
            .Select(id => CapabilityId.From(id))
            .ToList();

        var capabilityDrives = await _repository.FindAsync(
            cd => capabilityIds.Contains(cd.CapabilityId),
            cancellationToken);

        if (!capabilityDrives.Any())
            return Result.Success(new List<GetDrivesByCapabilityIdsResponseDtos>());

        // Get all unique drive IDs
        var driveIds = capabilityDrives
            .Select(cd => cd.DriveId)
            .Distinct()
            .ToList();

        // Fetch all drives with batch query, filtering for active only
        var drives = await _driveRepository.FindAsync(
            d => driveIds.Contains(d.Id) && d.IsActive,
            cancellationToken);

        // Map to DTOs and ensure distinct results
        var driveDtos = drives
            .Select(d => new GetDrivesByCapabilityIdsResponseDtos(
                d.Id.Value,
                d.Name,
                d.QuantityInStock,
                d.MinVolume!.Value))
            .Distinct()
            .ToList();

        return Result.Success(driveDtos);
    }
}
