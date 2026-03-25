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
        var allConfigs = await _repository.GetAllAsync(cancellationToken);
        var query = allConfigs.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(c =>
                c.CountryName.ToLower().Contains(searchTerm) ||
                c.CountryCode.ToLower().Contains(searchTerm));
        }

        // Apply pagination
        var totalCount = query.Count();
        var configs = query
            .OrderBy(c => c.CountryName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var configDtos = configs
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
