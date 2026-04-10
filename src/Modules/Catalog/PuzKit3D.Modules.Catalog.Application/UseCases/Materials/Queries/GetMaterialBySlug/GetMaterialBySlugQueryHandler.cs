using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetMaterialBySlug;

internal sealed class GetMaterialBySlugQueryHandler : IQueryHandler<GetMaterialBySlugQuery, object>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICurrentUser _currentUser;

    public GetMaterialBySlugQueryHandler(
        IMaterialRepository materialRepository,
        ICurrentUser currentUser)
    {
        _materialRepository = materialRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetMaterialBySlugQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get material by slug
        var material = await _materialRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (material is null)
        {
            return Result.Failure<object>(
                MaterialError.NotFoundBySlug(request.Slug));
        }

        // For non-staff/manager users, only return active materials
        if (!isStaffOrManager && !material.IsActive)
        {
            return Result.Failure<object>(
                MaterialError.NotFoundBySlug(request.Slug));
        }

        // Build response DTO based on user role
        object response = isStaffOrManager
            ? new GetMaterialDetailedResponseDto(
                material.Id.Value,
                material.Name,
                material.Slug,
                material.Description,
                material.IsActive,
                material.CreatedAt,
                material.UpdatedAt)
            : new GetMaterialResponseDto(
                material.Id.Value,
                material.Name,
                material.Slug,
                material.Description);
        return Result.Success(response);
    }
}
