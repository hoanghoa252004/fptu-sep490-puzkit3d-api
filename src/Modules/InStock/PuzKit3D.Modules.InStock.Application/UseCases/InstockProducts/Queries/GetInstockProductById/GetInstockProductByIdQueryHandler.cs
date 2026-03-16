using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductBySlug;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductById;

internal sealed class GetInstockProductByIdQueryHandler 
    : IQueryHandler<GetInstockProductByIdQuery, GetInstockProductByIdResponseDto>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly ICurrentUser _currentUser;

    public GetInstockProductByIdQueryHandler(IInstockProductRepository productRepository, ICurrentUser currentUser)
    {
        _productRepository = productRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<GetInstockProductByIdResponseDto>> Handle(
        GetInstockProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<GetInstockProductByIdResponseDto>(
                InstockProductError.NotFound(request.Id));
        }

        // Check if user is Staff or Business Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // For anonymous/customer users, only show active products
        if (!isStaffOrManager && !product.IsActive)
        {
            return Result.Failure<GetInstockProductByIdResponseDto>(
                Error.NotFound(
                    "InstockProduct.NotActive",
                    $"Instock product with Id '{request.Id}' is not available."));
        }

        var response = new GetInstockProductByIdResponseDto(
            product.Id.Value,
            product.Code,
            product.Slug,
            product.Name,
            product.TotalPieceCount,
            product.DifficultLevel,
            product.EstimatedBuildTime,
            product.ThumbnailUrl,
            product.PreviewAsset,
            product.Description,
            product.TopicId,
            product.AssemblyMethodId,
            product.GetCapabilityIds(),
            product.MaterialId,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);

        return Result.Success(response);
    }
}
