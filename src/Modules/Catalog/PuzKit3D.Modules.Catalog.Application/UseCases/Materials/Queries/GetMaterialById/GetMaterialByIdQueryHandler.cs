using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetMaterialById;

internal sealed class GetMaterialByIdQueryHandler : IQueryHandler<GetMaterialByIdQuery, GetMaterialByIdResponseDto>
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

    public async Task<ResultT<GetMaterialByIdResponseDto>> Handle(
        GetMaterialByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get material by ID
        var materialId = MaterialId.From(request.Id);
        var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);

        if (material is null)
        {
            return Result.Failure<GetMaterialByIdResponseDto>(
                MaterialError.NotFound(request.Id));
        }

        // For non-staff/manager users, only return active materials
        if (!isStaffOrManager && !material.IsActive)
        {
            return Result.Failure<GetMaterialByIdResponseDto>(
                MaterialError.NotFound(request.Id));
        }

        var response = new GetMaterialByIdResponseDto(
            Id: material.Id.Value,
            Name: material.Name,
            Slug: material.Slug,
            Description: material.Description,
            IsActive: material.IsActive,
            CreatedAt: material.CreatedAt,
            UpdatedAt: material.UpdatedAt);

        return Result.Success(response);
    }
}
