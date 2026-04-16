namespace PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;

public sealed record GetDrivesByCapabilityIdsResponseDtos(
    Guid Id,
    string Name,
    int Quantity,
    int MinVolume
);
