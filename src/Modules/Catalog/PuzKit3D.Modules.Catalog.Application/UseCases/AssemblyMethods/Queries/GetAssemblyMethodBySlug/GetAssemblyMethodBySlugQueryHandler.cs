using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

internal sealed class GetAssemblyMethodBySlugQueryHandler : IQueryHandler<GetAssemblyMethodBySlugQuery, object>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAssemblyMethodBySlugQueryHandler(IAssemblyMethodRepository assemblyMethodRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public async Task<ResultT<object>> Handle(
        GetAssemblyMethodBySlugQuery request, 
        CancellationToken cancellationToken)
    {
        // Get assembly method by slug - only return active ones for public access
        var assemblyMethod = await _assemblyMethodRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (assemblyMethod is null || !assemblyMethod.IsActive)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFoundBySlug(request.Slug));
        }

        // Map to public DTO (without timestamps and isActive)
        object response = new GetAssemblyMethodResponseDto(
            Id: assemblyMethod.Id.Value,
            Name: assemblyMethod.Name,
            Slug: assemblyMethod.Slug,
            Description: assemblyMethod.Description);

        return Result.Success(response);
    }
}


