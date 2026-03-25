using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.ActivateImportServiceConfig;

internal sealed class ActivateImportServiceConfigCommandHandler : ICommandHandler<ActivateImportServiceConfigCommand>
{
    private readonly IImportServiceConfigRepository _repository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public ActivateImportServiceConfigCommandHandler(
        IImportServiceConfigRepository repository,
        IPartnerUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ActivateImportServiceConfigCommand request,
        CancellationToken cancellationToken)
    {
        var config = await _repository.GetByIdAsync(
            ImportServiceConfigId.From(request.Id),
            cancellationToken);

        if (config is null)
        {
            return Result.Failure(ImportServiceConfigError.NotFound(request.Id));
        }

        if (config.IsActive)
        {
            return Result.Failure(ImportServiceConfigError.AlreadyActive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            config.Activate();
            _repository.Update(config);
            return Result.Success();
        }, cancellationToken);
    }
}
