using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetAllImportServiceConfigs;

internal sealed class GetAllImportServiceConfigsQueryHandler
    : IQueryHandler<GetAllImportServiceConfigsQuery, PagedResult<object>>
{
    private readonly IImportServiceConfigRepository _repository;

    public GetAllImportServiceConfigsQueryHandler(
        IImportServiceConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllImportServiceConfigsQuery request,
        CancellationToken cancellationToken)
    {
        // Get all import service configs
        var allConfigs = await _repository.GetAllAsync(request.SearchTerm, request.Ascending, cancellationToken);

        // Apply pagination
        var totalCount = allConfigs.Count();
        var configDtos = allConfigs
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => (object)new GetAllImportServiceConfigsResponseDto(
                c.Id.Value,
                c.BaseShippingFee,
                c.CountryCode,
                c.CountryName,
                c.ImportTaxPercentage,
                c.IsActive,
                c.CreatedAt,
                c.UpdatedAt))
            .ToList();

        var pagedResult = new PagedResult<object>(
            configDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
