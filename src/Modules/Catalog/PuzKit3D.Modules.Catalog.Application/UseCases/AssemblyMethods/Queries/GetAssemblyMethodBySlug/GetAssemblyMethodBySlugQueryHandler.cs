using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

internal sealed class GetAssemblyMethodBySlugQueryHandler : IQueryHandler<GetAssemblyMethodBySlugQuery, GetAssemblyMethodBySlugResponseDto>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAssemblyMethodBySlugQueryHandler(IAssemblyMethodRepository assemblyMethodRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public async Task<ResultT<GetAssemblyMethodBySlugResponseDto>> Handle(
        GetAssemblyMethodBySlugQuery request, 
        CancellationToken cancellationToken)
    {
        // Get assembly method by slug
        var assemblyMethod = await _assemblyMethodRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (assemblyMethod is null)
        {
            return Result.Failure<GetAssemblyMethodBySlugResponseDto>(
                AssemblyMethodError.NotFoundBySlug(request.Slug));
        }


        // Map to DTO
        var response = new GetAssemblyMethodBySlugResponseDto(
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
