using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetInstockProductVariantById;

internal sealed class GetInstockProductVariantByIdQueryHandler 
: IQueryHandler<GetInstockProductVariantByIdQuery, GetInstockProductVariantByIdResponseDto>
{
    private readonly IInstockProductVariantRepository _variantRepository;

    public GetInstockProductVariantByIdQueryHandler(IInstockProductVariantRepository variantRepository)
    {
        _variantRepository = variantRepository;
    }

    public async Task<ResultT<GetInstockProductVariantByIdResponseDto>> Handle(
        GetInstockProductVariantByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);

        if (variant is null)
        {
            return Result.Failure<GetInstockProductVariantByIdResponseDto>(
                InstockProductVariantError.NotFound(request.VariantId));
        }

        var response = new GetInstockProductVariantByIdResponseDto(
            variant.Id.Value,
            variant.InstockProductId.Value,
            variant.Sku,
            variant.Color,
            variant.AssembledLengthMm,
            variant.AssembledWidthMm,
            variant.AssembledHeightMm,
            variant.IsActive,
            variant.CreatedAt,
            variant.UpdatedAt);

        return Result.Success(response);
    }
}
