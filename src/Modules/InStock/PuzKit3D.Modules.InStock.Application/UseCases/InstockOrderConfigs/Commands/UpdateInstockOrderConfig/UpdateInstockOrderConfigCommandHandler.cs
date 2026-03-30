using MediatR;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrderConfigs.Commands.UpdateInstockOrderConfig;

internal sealed class UpdateInstockOrderConfigCommandHandler : ICommandHandler<UpdateInstockOrderConfigCommand>
{
    private readonly IInstockOrderConfigRepository _instockOrderConfigRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockOrderConfigCommandHandler(
        IInstockOrderConfigRepository instockOrderConfigRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _instockOrderConfigRepository = instockOrderConfigRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateInstockOrderConfigCommand request,
        CancellationToken cancellationToken)
    {
        var instockOrderConfig = await _instockOrderConfigRepository.GetFirstAsync(cancellationToken);

        if (instockOrderConfig is null)
        {
            return Result.Failure(
                InstockOrderError.InstockOrderConfigNotFound());
        }

        // Update only the field that is provided
        var updatedDays = request.OrderMustCompleteInDays ?? instockOrderConfig.OrderMustCompleteInDays;

        // Validate updatedDays must be at least 1
        if (updatedDays < 1)
        {
            return Result.Failure(
                InstockOrderError.InvalidOrderMustCompleteInDays());
        }

        // Validate updatedDays must not exceed 30
        if (updatedDays > 30)
        {
            return Result.Failure(
                InstockOrderError.OrderMustCompleteInDaysExceedsMaximum());
        }

        instockOrderConfig.Update(updatedDays);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
