using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodById;

internal sealed class GetAssemblyMethodByIdQueryHandler : IQueryHandler<GetAssemblyMethodByIdQuery, GetAssemblyMethodByIdResponseDto>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;

    public GetAssemblyMethodByIdQueryHandler(IAssemblyMethodRepository assemblyMethodRepository)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
    }

    public Task<ResultT<GetAssemblyMethodByIdResponseDto>> Handle(
        GetAssemblyMethodByIdQuery request, 
        CancellationToken cancellationToken)
    {
        // Get assembly method by ID
        var assemblyMethodId = AssemblyMethodId.From(request.Id);
        var assemblyMethod = _assemblyMethodRepository.FindById(assemblyMethodId);

        if (assemblyMethod is null)
        {
            return Task.FromResult(
                Result.Failure<GetAssemblyMethodByIdResponseDto>(
                    AssemblyMethodError.NotFound(request.Id)));
        }

        // Map to DTO
        var response = new GetAssemblyMethodByIdResponseDto(
            Id: assemblyMethod.Id.Value,
            Name: assemblyMethod.Name,
            Slug: assemblyMethod.Slug,
            Description: assemblyMethod.Description,
            IsActive: assemblyMethod.IsActive,
            CreatedAt: assemblyMethod.CreatedAt,
            UpdatedAt: assemblyMethod.UpdatedAt);

        return Task.FromResult(Result.Success(response));
    }
}
