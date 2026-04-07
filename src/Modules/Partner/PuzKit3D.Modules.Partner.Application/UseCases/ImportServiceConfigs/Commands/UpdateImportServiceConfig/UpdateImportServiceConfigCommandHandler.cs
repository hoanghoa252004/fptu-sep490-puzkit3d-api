using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.UpdateImportServiceConfig;

internal sealed class UpdateImportServiceConfigCommandHandler : ICommandHandler<UpdateImportServiceConfigCommand>
{
    private readonly IImportServiceConfigRepository _repository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public UpdateImportServiceConfigCommandHandler(
        IImportServiceConfigRepository repository,
        IPartnerUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateImportServiceConfigCommand request,
        CancellationToken cancellationToken)
    {
        var config = await _repository.GetByIdAsync(
            ImportServiceConfigId.From(request.Id),
            cancellationToken);

        if (config is null)
        {
            return Result.Failure(ImportServiceConfigError.NotFound(request.Id));
        }

        var updateResult = config.Update(
            request.BaseShippingFee,
            request.CountryCode,
            request.CountryName,
            request.ImportTaxPercentage,
            request.EstimatedDeliveryDays);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            _repository.Update(config);
            return Result.Success();
        }, cancellationToken);
    }
}
