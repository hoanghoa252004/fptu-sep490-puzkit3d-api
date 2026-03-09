using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetAllInstockProducts;

internal sealed class GetAllInstockProductsQueryHandler 
    : IQueryHandler<GetAllInstockProductsQuery, PagedResult<object>>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllInstockProductsQueryHandler(
        IInstockProductRepository productRepository,
        ICurrentUser currentUser)
    {
        _productRepository = productRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllInstockProductsQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Business Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        var query = allProducts.AsQueryable();

        // For anonymous/customer users, only show active products
        if (!isStaffOrManager)
        {
            query = query.Where(p => p.IsActive);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(searchTerm) || 
                p.Code.ToLower().Contains(searchTerm) ||
                p.Slug.ToLower().Contains(searchTerm) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
        }

        // Apply IsActive filter (only for staff/manager)
        if (isStaffOrManager && request.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == request.IsActive.Value);
        }

        var totalCount = query.Count();
        query = query.OrderBy(p => p.Code);

        IReadOnlyList<object> items;
        
        if (isStaffOrManager)
        {
            // Staff/Manager: Return FULL details with all fields including timestamps and IsActive
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetAllInstockProductsResponseDto(
                    p.Id.Value,
                    p.Code,
                    p.Slug,
                    p.Name,
                    p.TotalPieceCount,
                    p.DifficultLevel,
                    p.EstimatedBuildTime,
                    p.ThumbnailUrl,
                    p.Description,
                    p.IsActive,
                    p.CreatedAt,
                    p.UpdatedAt) as object)
                .ToList();
        }
        else
        {
            // Anonymous/Customer: Return PUBLIC details without timestamps, without IsActive flag
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetAllInstockProductsPublicResponseDto(
                    p.Id.Value,
                    p.Code,
                    p.Slug,
                    p.Name,
                    p.TotalPieceCount,
                    p.DifficultLevel,
                    p.EstimatedBuildTime,
                    p.ThumbnailUrl,
                    p.Description) as object)
                .ToList();
        }

        var pagedResult = PagedResult<object>.Create(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}

