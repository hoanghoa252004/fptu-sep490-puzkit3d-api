using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.Shared;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;

public sealed record GetCapabilityDriveResponseDto(
    Guid CapabilityId,
    Guid DriveId,
    GetDriveByIdResponseDto Drive);
