using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetAllInstockProductVariantsByProductId;

internal sealed class GetAllInstockProductVariantsByProductIdQueryHandler 
: IQueryHandler<GetAllInstockProductVariantsByProductIdQuery, GetAllInstockProductVariantsByProductIdResponseDto>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllInstockProductVariantsByProductIdQueryHandler(
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository,
        ICurrentUser currentUser)
    {
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<GetAllInstockProductVariantsByProductIdResponseDto>> Handle(
        GetAllInstockProductVariantsByProductIdQuery request, 
        CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<GetAllInstockProductVariantsByProductIdResponseDto>(
                InstockProductError.NotFound(request.ProductId));
        }

        var variants = await _variantRepository.GetAllByProductIdAsync(productId, cancellationToken);

        var isStaffOrManager = _currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager);

        if (!isStaffOrManager)
        {
            variants = variants.Where(v => v.IsActive);
        }

        // Sort by SKU ascending
        variants = variants.OrderBy(v => v.Sku);

        var variantDtos = variants.Select(v => new VariantDto(
            v.Id.Value,
            v.Sku,
            v.Color,
            v.AssembledLengthMm,
            v.AssembledWidthMm,
            v.AssembledHeightMm,
            v.IsActive,
            v.CreatedAt,
            v.UpdatedAt));

        var response = new GetAllInstockProductVariantsByProductIdResponseDto(variantDtos);

        return Result.Success(response);
    }
}
