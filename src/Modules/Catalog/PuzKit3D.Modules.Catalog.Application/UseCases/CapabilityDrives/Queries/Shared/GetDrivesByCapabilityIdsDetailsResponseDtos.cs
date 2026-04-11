using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.Shared;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;

public sealed record GetDrivesByCapabilityIdsDetailsResponseDtos (
    Guid CapabilityId,
    List<GetDriveByIdResponseDto> Drives);
