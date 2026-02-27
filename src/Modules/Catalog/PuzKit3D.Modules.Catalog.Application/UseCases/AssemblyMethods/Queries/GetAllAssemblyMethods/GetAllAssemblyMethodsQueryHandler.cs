using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;

internal sealed class GetAllAssemblyMethodsQueryHandler 
    : IQueryHandler<GetAllAssemblyMethodsQuery, PagedResult<object>>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllAssemblyMethodsQueryHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICurrentUser currentUser)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _currentUser = currentUser;
    }

    public Task<ResultT<PagedResult<object>>> Handle(
        GetAllAssemblyMethodsQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Build query with filters
        var query = _assemblyMethodRepository.FindAll(null);

        // For non-staff/manager users (anonymous or customer/admin), only show active items
        if (!isStaffOrManager)
        {
            query = query.Where(a => a.IsActive);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(a => 
                a.Name.ToLower().Contains(searchTerm) || 
                a.Slug.ToLower().Contains(searchTerm) ||
                (a.Description != null && a.Description.ToLower().Contains(searchTerm)));
        }

        // Apply IsActive filter (only for staff/manager)
        if (isStaffOrManager && request.IsActive.HasValue)
        {
            query = query.Where(a => a.IsActive == request.IsActive.Value);
        }

        // Get total count before paging
        var totalCount = query.Count();

        // Apply sorting (by name ascending by default)
        query = query.OrderBy(a => a.Name);

        // Apply paging and map to appropriate DTO based on user role
        IReadOnlyList<object> items;
        
        if (isStaffOrManager)
        {
            // Staff/Manager: Return full details with timestamps
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new GetAllAssemblyMethodsResponseDto(
                    a.Id.Value,
                    a.Name,
                    a.Slug,
                    a.Description,
                    a.IsActive,
                    a.CreatedAt,
                    a.UpdatedAt) as object)
                .ToList();
        }
        else
        {
            // Anonymous/Customer/Admin: Return public details without timestamps
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new GetAllAssemblyMethodsPublicResponseDto(
                    a.Id.Value,
                    a.Name,
                    a.Slug,
                    a.Description) as object)
                .ToList();
        }

        var pagedResult = PagedResult<object>.Create(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Task.FromResult(Result.Success(pagedResult));
    }
}
