using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Commands.UpdatePaymentConfig;

public record UpdatePaymentConfigCommand(
    int? OnlinePaymentExpiredInDays,
    int? OnlineTransactionExpiredInMinutes) : ICommand;
