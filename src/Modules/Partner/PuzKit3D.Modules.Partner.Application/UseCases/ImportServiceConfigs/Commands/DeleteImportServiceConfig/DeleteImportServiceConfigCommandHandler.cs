using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.DeleteImportServiceConfig;

internal sealed class DeleteImportServiceConfigCommandHandler : ICommandHandler<DeleteImportServiceConfigCommand>
{
    private readonly IImportServiceConfigRepository _repository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public DeleteImportServiceConfigCommandHandler(
        IImportServiceConfigRepository repository,
        IPartnerUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteImportServiceConfigCommand request,
        CancellationToken cancellationToken)
    {
        var config = await _repository.GetByIdAsync(
            ImportServiceConfigId.From(request.Id),
            cancellationToken);

        if (config is null)
        {
            return Result.Failure(ImportServiceConfigError.NotFound(request.Id));
        }

        if (!config.IsActive)
        {
            return Result.Failure(ImportServiceConfigError.AlreadyInactive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            config.Deactivate();
            _repository.Update(config);
            return Result.Success();
        }, cancellationToken);
    }
}
