using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPieceById;

public sealed record GetPieceByIdQuery(Guid ProductId, Guid PartId, Guid PieceId) 
    : IQuery<GetPieceByIdResponseDto>;
