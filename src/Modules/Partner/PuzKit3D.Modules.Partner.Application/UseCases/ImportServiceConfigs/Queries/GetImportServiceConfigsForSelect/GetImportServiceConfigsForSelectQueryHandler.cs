using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigsForSelect;

internal sealed class GetImportServiceConfigsForSelectQueryHandler
    : IQueryHandler<GetImportServiceConfigsForSelectQuery, IEnumerable<object>>
{
    private readonly IImportServiceConfigRepository _repository;

    public GetImportServiceConfigsForSelectQueryHandler(
        IImportServiceConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<IEnumerable<object>>> Handle(
        GetImportServiceConfigsForSelectQuery request,
        CancellationToken cancellationToken)
    {
        // Get all active import service configs
        var allConfigs = await _repository.GetAllAsync(cancellationToken);
        var configs = allConfigs
            .Where(c => c.IsActive)
            .OrderBy(c => c.CountryName)
            .ToList();

        // Build response DTOs with only Id, CountryName, CountryCode
        var configDtos = configs
            .Select(c => (object)new GetImportServiceConfigsForSelectResponseDto(
                c.Id.Value,
                c.CountryName,
                c.CountryCode))
            .ToList();

        return Result.Success((IEnumerable<object>)configDtos);
    }
}
