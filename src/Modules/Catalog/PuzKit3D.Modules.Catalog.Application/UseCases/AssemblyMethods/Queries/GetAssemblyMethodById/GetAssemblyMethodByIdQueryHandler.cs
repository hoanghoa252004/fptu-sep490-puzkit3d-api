using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodById;

internal sealed class GetAssemblyMethodByIdQueryHandler : IQueryHandler<GetAssemblyMethodByIdQuery, object>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAssemblyMethodByIdQueryHandler(IAssemblyMethodRepository assemblyMethodRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public async Task<ResultT<object>> Handle(
        GetAssemblyMethodByIdQuery request, 
        CancellationToken cancellationToken)
    {
        // Get assembly method by ID
        var assemblyMethodId = AssemblyMethodId.From(request.Id);
        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);

        if (assemblyMethod is null)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFound(request.Id));
        }

        // Map to DTO
        object response = new GetAssemblyMethodDetailedResponseDto(
            Id: assemblyMethod.Id.Value,
            Name: assemblyMethod.Name,
            Slug: assemblyMethod.Slug,
            Description: assemblyMethod.Description,
            IsActive: assemblyMethod.IsActive,
            CreatedAt: assemblyMethod.CreatedAt,
            UpdatedAt: assemblyMethod.UpdatedAt);

        return Result.Success(response);
    }
}

