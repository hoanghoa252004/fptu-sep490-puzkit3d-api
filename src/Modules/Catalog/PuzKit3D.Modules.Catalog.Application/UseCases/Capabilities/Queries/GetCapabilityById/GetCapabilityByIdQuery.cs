using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetCapabilityById;

public sealed record GetCapabilityByIdQuery(Guid Id) : IQuery<GetCapabilityByIdResponseDto>;

public sealed record GetCapabilityByIdResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
