using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetInstockProductVariantById;

public sealed record GetInstockProductVariantByIdQuery(Guid VariantId) : IQuery<GetInstockProductVariantByIdResponseDto>;
