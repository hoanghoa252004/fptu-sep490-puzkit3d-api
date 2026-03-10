using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetAllInstockProductVariantsByProductId;

public sealed record GetAllInstockProductVariantsByProductIdQuery(
    Guid ProductId) : IQuery<GetAllInstockProductVariantsByProductIdResponseDto>;
