using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailsByVariantId;

public sealed record GetInstockProductPriceDetailsByVariantIdQuery(
    Guid VariantId) : IQuery<object>;
