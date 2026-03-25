using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigById;

internal sealed class GetImportServiceConfigByIdQueryHandler
    : IQueryHandler<GetImportServiceConfigByIdQuery, object>
{
    private readonly IImportServiceConfigRepository _repository;

    public GetImportServiceConfigByIdQueryHandler(
        IImportServiceConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<object>> Handle(
        GetImportServiceConfigByIdQuery request,
        CancellationToken cancellationToken)
    {
        var config = await _repository.GetByIdAsync(
            ImportServiceConfigId.From(request.Id),
            cancellationToken);

        if (config is null)
        {
            return Result.Failure<object>(ImportServiceConfigError.NotFound(request.Id));
        }

        var responseDto = new GetImportServiceConfigByIdResponseDto(
            config.Id.Value,
            config.BaseShippingFee,
            config.CountryCode,
            config.CountryName,
            config.ImportTaxPercentage,
            config.IsActive,
            config.CreatedAt,
            config.UpdatedAt);

        return Result.Success((object)responseDto);
    }
}
