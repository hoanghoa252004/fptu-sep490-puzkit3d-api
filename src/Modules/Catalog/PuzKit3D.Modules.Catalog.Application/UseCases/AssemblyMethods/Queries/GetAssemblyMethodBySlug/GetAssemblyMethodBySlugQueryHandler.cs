using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

internal sealed class GetAssemblyMethodBySlugQueryHandler : IQueryHandler<GetAssemblyMethodBySlugQuery, GetAssemblyMethodBySlugPublicResponseDto>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAssemblyMethodBySlugQueryHandler(IAssemblyMethodRepository assemblyMethodRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public async Task<ResultT<GetAssemblyMethodBySlugPublicResponseDto>> Handle(
        GetAssemblyMethodBySlugQuery request, 
        CancellationToken cancellationToken)
    {
        // Get assembly method by slug - only return active ones for public access
        var assemblyMethod = await _assemblyMethodRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (assemblyMethod is null || !assemblyMethod.IsActive)
        {
            return Result.Failure<GetAssemblyMethodBySlugPublicResponseDto>(
                AssemblyMethodError.NotFoundBySlug(request.Slug));
        }

        // Map to public DTO (without timestamps and isActive)
        var response = new GetAssemblyMethodBySlugPublicResponseDto(
            Id: assemblyMethod.Id.Value,
            Name: assemblyMethod.Name,
            Slug: assemblyMethod.Slug,
            Description: assemblyMethod.Description);

        return Result.Success(response);
    }
}


