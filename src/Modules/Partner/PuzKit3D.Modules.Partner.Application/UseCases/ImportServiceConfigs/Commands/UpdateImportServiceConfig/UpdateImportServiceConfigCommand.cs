using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.UpdateImportServiceConfig;

public sealed record UpdateImportServiceConfigCommand(
    Guid Id,
    decimal BaseShippingFee,
    string CountryCode,
    string CountryName,
    decimal ImportTaxPercentage,
    int EstimatedDeliveryDays) : ICommand;
