using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductById;

internal sealed class GetPartnerProductByIdQueryHandler : IQueryHandler<GetPartnerProductByIdQuery, object>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly ICurrentUser _currentUser;

    public GetPartnerProductByIdQueryHandler(
        IPartnerProductRepository partnerProductRepository,
        ICurrentUser currentUser)
    {
        _partnerProductRepository = partnerProductRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _partnerProductRepository.GetByIdAsync(
            PartnerProductId.From(request.Id),
            cancellationToken);

        if (product is null)
        {
            return Result.Failure<object>(
                PartnerProductError.NotFound(request.Id));
        }

        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // For non-staff/manager users, only show active products
        if (!isStaffOrManager && !product.IsActive)
        {
            return Result.Failure<object>(
                Error.NotFound(
                    "PartnerProduct.NotActive",
                    $"Partner product with Id '{request.Id}' is not available."));
        }

        // Build response DTO based on user role
        object response = isStaffOrManager
            ? new GetPartnerProductByIdResponseDto
            (
                product.Id.Value,
                product.PartnerId.Value,
                product.Name,
                product.ReferencePrice,
                product.ThumbnailUrl,
                product.PreviewAsset,
                product.Slug,
                product.Description,
                product.IsActive,
                product.CreatedAt,
                product.UpdatedAt
            )
            : new GetPartnerProductByIdForCustomerResponseDto
            (
                product.Id.Value,
                product.PartnerId.Value,
                product.Name,
                product.ReferencePrice,
                product.ThumbnailUrl,
                product.PreviewAsset,
                product.Slug,
                product.Description
            );

        return Result.Success(response);
    }
}
