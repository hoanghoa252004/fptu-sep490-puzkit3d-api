using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;

public static class ImportServiceConfigError
{
    // Base Shipping Fee
    public static Error InvalidBaseShippingFee() => Error.Validation(
        "ImportServiceConfig.InvalidBaseShippingFee",
        "Base shipping fee must be greater than or equal to 20000.");

    // Country Code
    public static Error EmptyCountryCode() => Error.Validation(
        "ImportServiceConfig.EmptyCountryCode",
        "Country code cannot be empty.");

    public static Error DuplicateCountryCode(string code) => Error.Conflict(
        "ImportServiceConfig.DuplicateCountryCode",
        $"Import service config with country code '{code}' already exists.");

    // Country Name
    public static Error EmptyCountryName() => Error.Validation(
        "ImportServiceConfig.EmptyCountryName",
        "Country name cannot be empty.");

    // Import Tax Percentage
    public static Error InvalidImportTaxPercentage() => Error.Validation(
        "ImportServiceConfig.InvalidImportTaxPercentage",
        "Import tax percentage must be between 0 and 100.");

    // Import Service Config
    public static Error NotFound(Guid id) => Error.NotFound(
        "ImportServiceConfig.NotFound",
        $"Import service config with ID '{id}' was not found.");

    public static Error AlreadyInactive(Guid id) => Error.Conflict(
        "ImportServiceConfig.AlreadyInactive",
        $"Import service config with ID '{id}' is already inactive.");

    public static Error AlreadyActive(Guid id) => Error.Conflict(
        "ImportServiceConfig.AlreadyActive",
        $"Import service config with ID '{id}' is already active.");
}
