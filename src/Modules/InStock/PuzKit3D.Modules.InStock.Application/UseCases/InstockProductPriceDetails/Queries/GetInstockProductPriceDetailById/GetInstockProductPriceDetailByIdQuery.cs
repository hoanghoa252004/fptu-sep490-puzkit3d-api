using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailById;

public sealed record GetInstockProductPriceDetailByIdQuery(Guid PriceDetailId) : IQuery<GetInstockProductPriceDetailByIdResponseDto>;
