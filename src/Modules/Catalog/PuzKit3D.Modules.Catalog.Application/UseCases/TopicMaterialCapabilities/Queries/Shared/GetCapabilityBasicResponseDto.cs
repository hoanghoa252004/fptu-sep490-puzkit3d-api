namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;

public sealed record GetCapabilityBasicResponseDto(
    Guid Id,
    string Name,
    string Slug);
