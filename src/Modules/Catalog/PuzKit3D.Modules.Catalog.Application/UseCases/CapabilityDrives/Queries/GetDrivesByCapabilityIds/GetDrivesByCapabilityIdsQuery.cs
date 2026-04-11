using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetDrivesByCapabilityIds;

public sealed record GetDrivesByCapabilityIdsQuery(
    List<Guid> CapabilityIds) : IQuery<List<GetDriveBasicResponseDto>>;
