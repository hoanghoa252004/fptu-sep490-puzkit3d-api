using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetAllDrives;

public sealed record GetAllDrivesQuery : IQuery<IEnumerable<GetAllDrivesResponseDto>>;

public sealed record GetAllDrivesResponseDto(
Guid Id,
string Name,
string? Description,
int? MinVolume,
int QuantityInStock,
bool IsActive);
