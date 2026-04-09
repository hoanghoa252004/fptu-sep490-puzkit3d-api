using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetCapabilityDriveById;

public sealed record GetCapabilityDriveByIdQuery(Guid Id) : IQuery<GetCapabilityDriveByIdResponseDto>;

public sealed record GetCapabilityDriveByIdResponseDto(
    Guid Id,
    Guid CapabilityId,
    Guid DriveId,
    int Quantity);
