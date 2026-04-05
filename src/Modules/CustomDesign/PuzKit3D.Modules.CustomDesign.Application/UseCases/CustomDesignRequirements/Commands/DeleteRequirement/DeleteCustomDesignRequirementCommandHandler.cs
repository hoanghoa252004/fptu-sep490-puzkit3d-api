using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.DeleteRequirement;

internal sealed class DeleteCustomDesignRequirementCommandHandler : ICommandHandler<DeleteCustomDesignRequirementCommand>
{
    private readonly ICustomDesignRequirementRepository _repository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;

    public DeleteCustomDesignRequirementCommandHandler(
        ICustomDesignRequirementRepository repository,
        ICustomDesignUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteCustomDesignRequirementCommand request,
        CancellationToken cancellationToken)
    {

        var requirementResult = await _repository.GetByIdAsync(CustomDesignRequirementId.From(request.Id), cancellationToken);

        if (requirementResult.IsFailure)
            return Result.Failure<CustomDesignRequirement>(requirementResult.Error);

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _repository.Delete(requirementResult.Value);
            return Result.Success();
        }, cancellationToken);
    }
}
