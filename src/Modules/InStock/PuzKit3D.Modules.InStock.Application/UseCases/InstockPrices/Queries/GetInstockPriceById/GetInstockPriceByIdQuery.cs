using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetInstockPriceById;

public sealed record GetInstockPriceByIdQuery(Guid PriceId) : IQuery<GetInstockPriceByIdResponseDto>;
