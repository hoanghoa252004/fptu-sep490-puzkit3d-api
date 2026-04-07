using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.CreateImportServiceConfig;

public sealed record CreateImportServiceConfigCommand(
    decimal BaseShippingFee,
    string CountryCode,
    string CountryName,
    decimal ImportTaxPercentage, 
    int EstimatedDeliveryDays) : ICommandT<Guid>;
