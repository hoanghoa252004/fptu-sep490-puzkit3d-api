using MediatR;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;
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

        // Parse string units to TimeUnit enum
        TimeUnit? parsedPaymentUnit = null;
        TimeUnit? parsedTransactionUnit = null;

        if (!string.IsNullOrWhiteSpace(request.OnlinePaymentExpiredUnit))
        {
            if (!Enum.TryParse<TimeUnit>(request.OnlinePaymentExpiredUnit, ignoreCase: true, out var unit))
            {
                return Result.Failure(
                    SharedKernel.Domain.Errors.Error.Validation(
                        "PaymentConfig.InvalidOnlinePaymentExpiredUnit",
                        $"Invalid time unit '{request.OnlinePaymentExpiredUnit}'. Valid units are: Minute, Hour, Day"));
            }
            parsedPaymentUnit = unit;
        }

        if (!string.IsNullOrWhiteSpace(request.OnlineTransactionExpiredUnit))
        {
            if (!Enum.TryParse<TimeUnit>(request.OnlineTransactionExpiredUnit, ignoreCase: true, out var unit))
            {
                return Result.Failure(
                    SharedKernel.Domain.Errors.Error.Validation(
                        "PaymentConfig.InvalidOnlineTransactionExpiredUnit",
                        $"Invalid time unit '{request.OnlineTransactionExpiredUnit}'. Valid units are: Minute, Hour, Day"));
            }
            parsedTransactionUnit = unit;
        }

        // Update only the fields that are provided
        var updatedPaymentValue = request.OnlinePaymentExpiredValue ?? paymentConfig.OnlinePaymentExpiredValue;
        var updatedPaymentUnit = parsedPaymentUnit ?? paymentConfig.OnlinePaymentExpiredUnit;
        var updatedTransactionValue = request.OnlineTransactionExpiredValue ?? paymentConfig.OnlineTransactionExpiredValue;
        var updatedTransactionUnit = parsedTransactionUnit ?? paymentConfig.OnlineTransactionExpiredUnit;

        // Validate online payment expiration value
        if (updatedPaymentValue <= 0 || updatedTransactionValue <= 0)
        {
            return Result.Failure(
                SharedKernel.Domain.Errors.Error.Validation(
                    "PaymentConfig.InvalidValue",
                    "Both online payment and transaction expiration values must be greater than 0"));
        }
        // Validate transaction expiration must be less than payment expiration
        var paymentExpirationInMinutes = ConvertToMinutes(updatedPaymentValue, updatedPaymentUnit);
        var transactionExpirationInMinutes = ConvertToMinutes(updatedTransactionValue, updatedTransactionUnit);

        if (transactionExpirationInMinutes >= paymentExpirationInMinutes)
        {
            return Result.Failure(
                SharedKernel.Domain.Errors.Error.Validation(
                    "PaymentConfig.InvalidExpirationOrder",
                    $"Transaction expiration ({updatedTransactionValue} {updatedTransactionUnit}) must be less than payment expiration ({updatedPaymentValue} {updatedPaymentUnit})"));
        }

        paymentConfig.Update(
            updatedPaymentValue,
            updatedPaymentUnit,
            updatedTransactionValue,
            updatedTransactionUnit);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static int ConvertToMinutes(int value, TimeUnit unit)
    {
        return unit switch
        {
            TimeUnit.Minute => value,
            TimeUnit.Hour => value * 60,
            TimeUnit.Day => value * 24 * 60,
            _ => value
        };
    }
}


