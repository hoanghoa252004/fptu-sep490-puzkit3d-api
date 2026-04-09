using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetDriveById;

public sealed record GetDriveByIdQuery(Guid Id) : IQuery<GetDriveByIdResponseDto>;

public sealed record GetDriveByIdResponseDto(
Guid Id,
string Name,
string? Description,
int? MinVolume,
int QuantityInStock,
bool IsActive,
DateTime CreatedAt,
DateTime UpdatedAt);
