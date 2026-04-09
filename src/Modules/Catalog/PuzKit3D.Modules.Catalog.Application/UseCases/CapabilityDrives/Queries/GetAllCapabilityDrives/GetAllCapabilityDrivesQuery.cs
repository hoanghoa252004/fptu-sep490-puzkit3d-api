using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetAllCapabilityDrives;

public sealed record GetAllCapabilityDrivesQuery : IQuery<IEnumerable<GetAllCapabilityDrivesResponseDto>>;

public sealed record GetAllCapabilityDrivesResponseDto(
    Guid Id,
    Guid CapabilityId,
    Guid DriveId,
    int Quantity);
