using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Queries.GetInstockInventoryByVariantId;

public sealed record GetInstockInventoryByVariantIdQuery(
    Guid ProductId,
    Guid VariantId) : IQuery<GetInstockInventoryByVariantIdResponseDto>;
