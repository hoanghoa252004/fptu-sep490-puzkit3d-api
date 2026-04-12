using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetMaterialById;

internal sealed class GetMaterialByIdQueryHandler : IQueryHandler<GetMaterialByIdQuery, object>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICurrentUser _currentUser;

    public GetMaterialByIdQueryHandler(
        IMaterialRepository materialRepository,
        ICurrentUser currentUser)
    {
        _materialRepository = materialRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetMaterialByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get material by ID
        var materialId = MaterialId.From(request.Id);
        var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);

        if (material is null)
        {
            return Result.Failure<object>(
                MaterialError.NotFound(request.Id));
        }

        // For non-staff/manager users, only return active materials
        if (!isStaffOrManager && !material.IsActive)
        {
            return Result.Failure<object>(
                MaterialError.NotFound(request.Id));
        }

        // Build response DTO based on user role
        object response = isStaffOrManager
            ? new GetMaterialDetailedResponseDto(
                material.Id.Value,
                material.Name,
                material.Slug,
                material.Description,
                material.FactorPercentage,
                material.BasePrice,
                material.IsActive,
                material.CreatedAt,
                material.UpdatedAt)
            : new GetMaterialResponseDto(
                material.Id.Value,
                material.Name,
                material.Slug,
                material.Description,
                material.FactorPercentage,
                material.BasePrice);
        return Result.Success(response);
    }
}
