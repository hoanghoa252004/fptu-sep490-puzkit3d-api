using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Media;
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
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;
    private readonly IMediaAssetService _assetUrlService;

    public GetAllInstockProductsQueryHandler(
        IInstockProductRepository productRepository,
        ICurrentUser currentUser,
        ITopicReplicaRepository topicReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository,
        IMediaAssetService assetUrlService)
    {
        _productRepository = productRepository;
        _currentUser = currentUser;
        _topicReplicaRepository = topicReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
        _assetUrlService = assetUrlService;
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
            
            // Get all replicas for search
            var topics = await _topicReplicaRepository.GetAllAsync(cancellationToken);
            var materials = await _materialReplicaRepository.GetAllAsync(cancellationToken);
            var assemblyMethods = await _assemblyMethodReplicaRepository.GetAllAsync(cancellationToken);
            var capabilities = await _capabilityReplicaRepository.GetAllAsync(cancellationToken);

            // Find matching replica IDs
            var matchingTopicIds = topics
                .Where(t => t.Name.ToLower().Contains(searchTerm) || t.Slug.ToLower().Contains(searchTerm))
                .Select(t => t.Id)
                .ToList();

            var matchingMaterialIds = materials
                .Where(m => m.Name.ToLower().Contains(searchTerm) || m.Slug.ToLower().Contains(searchTerm))
                .Select(m => m.Id)
                .ToList();

            var matchingAssemblyMethodIds = assemblyMethods
                .Where(a => a.Name.ToLower().Contains(searchTerm) || a.Slug.ToLower().Contains(searchTerm))
                .Select(a => a.Id)
                .ToList();

            var matchingCapabilityIds = capabilities
                .Where(c => c.Name.ToLower().Contains(searchTerm) || c.Slug.ToLower().Contains(searchTerm))
                .Select(c => c.Id)
                .ToList();

            // Filter products by search term (product fields + related entities)
            query = query.Where(p =>
                // Product fields
                p.Name.ToLower().Contains(searchTerm) ||
                p.Code.ToLower().Contains(searchTerm) ||
                p.Slug.ToLower().Contains(searchTerm) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                // Topic search
                (matchingTopicIds.Count > 0 && matchingTopicIds.Contains(p.TopicId)) ||
                // Material search
                (matchingMaterialIds.Count > 0 && matchingMaterialIds.Contains(p.MaterialId)) ||
                // Assembly method search
                (matchingAssemblyMethodIds.Count > 0 && p.AssemblyMethodDetails.Any(c => matchingAssemblyMethodIds.Contains(c.AssemblyMethodId))) ||
                // Capability search
                (matchingCapabilityIds.Count > 0 && p.CapabilityDetails.Any(c => matchingCapabilityIds.Contains(c.CapabilityId))));
        }

        // Apply IsActive filter (only for staff/manager)
        if (isStaffOrManager && request.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == request.IsActive.Value);
        }

        // Apply DifficultyLevel filter
        if (!string.IsNullOrWhiteSpace(request.DifficultyLevel))
        {
            var difficultyLevel = request.DifficultyLevel.ToLower();
            query = query.Where(p => p.DifficultLevel.ToLower() == difficultyLevel);
        }

        // Apply Material filter by slug
        if (!string.IsNullOrWhiteSpace(request.MaterialSlug))
        {
            var materials = await _materialReplicaRepository.GetAllAsync(cancellationToken);
            var materialId = materials
                .FirstOrDefault(m => m.Slug.ToLower() == request.MaterialSlug.ToLower())
                ?.Id;

            if (materialId.HasValue)
            {
                query = query.Where(p => p.MaterialId == materialId.Value);
            }
            else
            {
                // No matching material found, return empty results
                return Result.Success(PagedResult<object>.Create(
                    new List<object>(),
                    request.PageNumber,
                    request.PageSize,
                    0));
            }
        }

        // Apply Topic filter by slug
        if (!string.IsNullOrWhiteSpace(request.TopicSlug))
        {
            var topics = await _topicReplicaRepository.GetAllAsync(cancellationToken);
            var topicId = topics
                .FirstOrDefault(t => t.Slug.ToLower() == request.TopicSlug.ToLower())
                ?.Id;

            if (topicId.HasValue)
            {
                query = query.Where(p => p.TopicId == topicId.Value);
            }
            else
            {
                // No matching topic found, return empty results
                return Result.Success(PagedResult<object>.Create(
                    new List<object>(),
                    request.PageNumber,
                    request.PageSize,
                    0));
            }
        }

        // Apply Assembly Method filter by slugs (product must have all specified assembly methods)
        if (request.AssemblyMethodSlug != null && request.AssemblyMethodSlug.Count > 0)
        {
            var assemblies = await _assemblyMethodReplicaRepository.GetAllAsync(cancellationToken);
            var assemblyIds = assemblies
                .Where(c => request.AssemblyMethodSlug.Any(slug => slug.ToLower() == c.Slug.ToLower()))
                .Select(c => c.Id)
                .ToList();

            if (assemblyIds.Count > 0)
            {
                // Filter products that have all specified assembly methods
                query = query.Where(p => assemblyIds.All(assemblyId => p.AssemblyMethodDetails.Any(cd => cd.AssemblyMethodId == assemblyId)));
            }
            else
            {
                // No matching assembly methods found, return empty results
                return Result.Success(PagedResult<object>.Create(
                    new List<object>(),
                    request.PageNumber,
                    request.PageSize,
                    0));
            }
        }

        // Apply Capability filter by slugs (product must have all specified capabilities)
        if (request.CapabilitySlugs != null && request.CapabilitySlugs.Count > 0)
        {
            var capabilities = await _capabilityReplicaRepository.GetAllAsync(cancellationToken);
            var capabilityIds = capabilities
                .Where(c => request.CapabilitySlugs.Any(slug => slug.ToLower() == c.Slug.ToLower()))
                .Select(c => c.Id)
                .ToList();

            if (capabilityIds.Count > 0)
            {
                // Filter products that have all specified capabilities
                query = query.Where(p => capabilityIds.All(capId => p.CapabilityDetails.Any(cd => cd.CapabilityId == capId)));
            }
            else
            {
                // No matching capabilities found, return empty results
                return Result.Success(PagedResult<object>.Create(
                    new List<object>(),
                    request.PageNumber,
                    request.PageSize,
                    0));
            }
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
                    _assetUrlService.BuildAssetUrl(p.ThumbnailUrl),
                    p.Description,
                    p.IsActive,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.TopicId,
                    p.MaterialId,
                    p.GetAssemblyMethodIds(),
                    p.GetCapabilityIds()) as object)
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
                    _assetUrlService.BuildAssetUrl(p.ThumbnailUrl),
                    p.Description,
                    p.TopicId,
                    p.MaterialId,
                    p.GetAssemblyMethodIds(),
                    p.GetCapabilityIds()) as object)
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




