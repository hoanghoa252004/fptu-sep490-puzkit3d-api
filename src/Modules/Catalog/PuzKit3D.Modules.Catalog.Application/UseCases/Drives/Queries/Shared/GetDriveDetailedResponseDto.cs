namespace PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.Shared;

public sealed record GetDriveDetailedResponseDto(
    Guid Id,
    string Name,
    string? Description,
    int? MinVolume,
    int QuantityInStock,
    bool IsActive);
