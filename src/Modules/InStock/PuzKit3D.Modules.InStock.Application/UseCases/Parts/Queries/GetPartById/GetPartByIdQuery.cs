using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartById;

public sealed record GetPartByIdQuery(Guid ProductId, Guid PartId) 
    : IQuery<GetPartByIdResponseDto>;
