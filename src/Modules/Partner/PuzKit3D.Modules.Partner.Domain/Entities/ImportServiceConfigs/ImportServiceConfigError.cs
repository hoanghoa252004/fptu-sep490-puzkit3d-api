using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;

public static class ImportServiceConfigError
{
    public static Error InvalidCode() => Error.Validation(
        "ImportServiceConfig.InvalidCode",
        "Import service config code cannot be empty.");

    public static Error InvalidBaseShippingFee() => Error.Validation(
        "ImportServiceConfig.InvalidBaseShippingFee",
        "Base shipping fee must be greater than or equal to 0.");

    public static Error InvalidImportTaxPercentage() => Error.Validation(
        "ImportServiceConfig.InvalidImportTaxPercentage",
        "Import tax percentage must be between 0 and 100.");

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
