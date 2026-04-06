using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets.DomainEvents;

public sealed record CustomDesignAssetCreatedDomainEvent(
    Guid AssetId,
    Guid RequestId,
    int Version) : DomainEvent;

