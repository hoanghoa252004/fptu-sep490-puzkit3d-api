using MediatR;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Commands.UpdatePaymentConfig;

internal sealed class UpdatePaymentConfigCommandHandler : ICommandHandler<UpdatePaymentConfigCommand>
{
    private readonly IPaymentConfigRepository _paymentConfigRepository;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public UpdatePaymentConfigCommandHandler(
        IPaymentConfigRepository paymentConfigRepository,
        IPaymentUnitOfWork unitOfWork)
    {
        _paymentConfigRepository = paymentConfigRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdatePaymentConfigCommand request,
        CancellationToken cancellationToken)
    {
        var paymentConfig = await _paymentConfigRepository.GetFirstAsync(cancellationToken);

        if (paymentConfig is null)
        {
            return Result.Failure(
                PaymentError.PaymentConfigNotFound());
        }

        // Update only the fields that are provided
        var updatedDays = request.OnlinePaymentExpiredInDays ?? paymentConfig.OnlinePaymentExpiredInDays;
        var updatedMinutes = request.OnlineTransactionExpiredInMinutes ?? paymentConfig.OnlineTransactionExpiredInMinutes;

        // Validate updatedDays must be at least 1
        if (updatedDays < 1 || updatedDays > 10)
        {
            return Result.Failure(
                PaymentError.InvalidOnlinePaymentExpiredInDays());
        }

        // Validate updatedMinutes must be at least 5
        if (updatedMinutes < 5 || updatedMinutes > 60)
        {
            return Result.Failure(
                PaymentError.InvalidOnlineTransactionExpiredInMinutes());
        }

        paymentConfig.Update(updatedDays, updatedMinutes);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
