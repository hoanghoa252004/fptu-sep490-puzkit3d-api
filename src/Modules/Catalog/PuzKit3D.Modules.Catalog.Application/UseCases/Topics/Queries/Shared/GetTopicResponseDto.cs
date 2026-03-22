namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;

public sealed record GetTopicResponseDto(
    Guid Id,
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description);
