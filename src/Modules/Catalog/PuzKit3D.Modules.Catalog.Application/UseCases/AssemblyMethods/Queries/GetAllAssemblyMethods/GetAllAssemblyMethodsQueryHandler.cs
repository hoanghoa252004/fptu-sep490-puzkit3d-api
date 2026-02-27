using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;

internal sealed class GetAllAssemblyMethodsQueryHandler 
    : IQueryHandler<GetAllAssemblyMethodsQuery, PagedResult<GetAllAssemblyMethodsResponseDto>>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAllAssemblyMethodsQueryHandler(IAssemblyMethodRepository assemblyMethodRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public Task<ResultT<PagedResult<GetAllAssemblyMethodsResponseDto>>> Handle(
        GetAllAssemblyMethodsQuery request,
        CancellationToken cancellationToken)
    {
        // Build query with filters
        var query = _assemblyMethodRepository.FindAll(null);

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(a => 
                a.Name.ToLower().Contains(searchTerm) || 
                a.Slug.ToLower().Contains(searchTerm) ||
                (a.Description != null && a.Description.ToLower().Contains(searchTerm)));
        }

        // Apply IsActive filter
        if (request.IsActive.HasValue)
        {
            query = query.Where(a => a.IsActive == request.IsActive.Value);
        }

        // Get total count before paging
        var totalCount = query.Count();

        // Apply sorting (by name ascending by default)
        query = query.OrderBy(a => a.Name);

        // Apply paging and map to DTO
        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new GetAllAssemblyMethodsResponseDto(
                a.Id.Value,
                a.Name,
                a.Slug,
                a.Description,
                a.IsActive,
                a.CreatedAt,
                a.UpdatedAt))
            .ToList();

        var pagedResult = PagedResult<GetAllAssemblyMethodsResponseDto>.Create(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Task.FromResult(Result.Success(pagedResult));
    }
}

