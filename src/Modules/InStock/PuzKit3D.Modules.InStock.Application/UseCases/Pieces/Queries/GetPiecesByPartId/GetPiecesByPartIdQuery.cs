using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPiecesByPartId;

public sealed record GetPiecesByPartIdQuery(
    Guid ProductId,
    Guid PartId) : IQuery<IReadOnlyList<GetPiecesByPartIdResponseDto>>;
