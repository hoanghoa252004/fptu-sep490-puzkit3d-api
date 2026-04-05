using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.Services;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.CreateRequest;

internal sealed class CreateCustomDesignRequestCommandHandler : ICommandTHandler<CreateCustomDesignRequestCommand, Guid>
{
    private readonly ICustomDesignRequestRepository _repository;
    private readonly ICustomDesignRequestCodeGenerator _codeGenerator;
    private readonly ICustomDesignRequirementRepository _requirementRepository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateCustomDesignRequestCommandHandler(
        ICustomDesignRequestRepository repository,
        ICustomDesignRequestCodeGenerator codeGenerator,
        ICustomDesignRequirementRepository requirementRepository,
        ICustomDesignUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _codeGenerator = codeGenerator;
        _requirementRepository = requirementRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateCustomDesignRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Validate requirement exists
        var requirementResult = await _requirementRepository.GetByIdAsync(
            CustomDesignRequirementId.From(request.CustomDesignRequirementId),
            cancellationToken);

        if (requirementResult.IsFailure)
            return Result.Failure<Guid>(requirementResult.Error);

        var customerId = Guid.Parse(_currentUser.UserId!);
        var code = await _codeGenerator.GenerateCodeAsync(cancellationToken);

        // Create request - validation happens in domain
        var createResult = CustomDesignRequest.Create(
            Guid.NewGuid(),
            code,
            customerId,
            request.CustomDesignRequirementId,
            request.DesiredLengthMm,
            request.DesiredWidthMm,
            request.DesiredHeightMm,
            request.Sketches,
            request.CustomerPrompt,
            request.DesiredDeliveryDate,
            request.DesiredQuantity,
            request.TargetBudget,
            request.Type,
            DateTime.UtcNow,
            DateTime.UtcNow);

        if (!createResult.IsSuccess)
            return Result.Failure<Guid>(createResult.Error);

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            await _repository.AddAsync(createResult.Value, cancellationToken);
            return Result.Success(createResult.Value.Id.Value);
        });
    }
}

