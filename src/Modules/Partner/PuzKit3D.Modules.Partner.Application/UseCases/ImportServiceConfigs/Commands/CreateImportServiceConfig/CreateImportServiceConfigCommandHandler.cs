using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.CreateImportServiceConfig;

internal sealed class CreateImportServiceConfigCommandHandler : ICommandTHandler<CreateImportServiceConfigCommand, Guid>
{
    private readonly IImportServiceConfigRepository _repository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public CreateImportServiceConfigCommandHandler(
        IImportServiceConfigRepository repository,
        IPartnerUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateImportServiceConfigCommand request,
        CancellationToken cancellationToken)
    {
        var existingCountryCode = await _repository.GetByCountryCodeAsync(request.CountryCode, cancellationToken);
        if(existingCountryCode is not null)
        {
            return Result.Failure<Guid>(ImportServiceConfigError.DuplicateCountryCode(request.CountryCode));
        }

        var configResult = Domain.Entities.ImportServiceConfigs.ImportServiceConfig.Create(
            request.BaseShippingFee,
            request.CountryCode,
            request.CountryName,
            request.ImportTaxPercentage,
            isActive: true);

        if (configResult.IsFailure)
        {
            return Result.Failure<Guid>(configResult.Error);
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _repository.Add(configResult.Value);
            return Result.Success(configResult.Value.Id.Value);
        }, cancellationToken);
    }
}
