using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartsByProductId;

public sealed record GetPartsByProductIdQuery(
    Guid ProductId) : IQuery<IReadOnlyList<GetPartsByProductIdResponseDto>>;
