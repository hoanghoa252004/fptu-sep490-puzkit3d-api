using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;

public class ImportServiceConfig : AggregateRoot<ImportServiceConfigId>
{
    public decimal BaseShippingFee { get; private set; }
    public string CountryCode { get; private set; } = null!;
    public string CountryName { get; private set; } = null!;
    public decimal ImportTaxPercentage { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ImportServiceConfig(
        ImportServiceConfigId id,
        decimal baseShippingFee,
        string countryCode,
        string countryName,
        decimal importTaxPercentage,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        BaseShippingFee = baseShippingFee;
        CountryCode = countryCode;
        CountryName = countryName;
        ImportTaxPercentage = importTaxPercentage;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private ImportServiceConfig() : base()
    {
    }

    public static ResultT<ImportServiceConfig> Create(
        decimal baseShippingFee,
        string countryCode,
        string countryName,
        decimal importTaxPercentage,
        bool isActive = false)
    {
        if (baseShippingFee < 0)
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.InvalidBaseShippingFee());

        if (importTaxPercentage < 0 || importTaxPercentage > 100)
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.InvalidImportTaxPercentage());

        var configId = ImportServiceConfigId.Create();

        var config = new ImportServiceConfig(
            configId,
            baseShippingFee,
            countryCode,
            countryName,
            importTaxPercentage,
            isActive,
            DateTime.UtcNow);

        return Result.Success(config);
    }

    public Result Update(
        decimal baseShippingFee,
        string countryCode,
        string countryName,
        decimal importTaxPercentage)
    {
        if (baseShippingFee < 0)
            return Result.Failure(ImportServiceConfigError.InvalidBaseShippingFee());

        if (importTaxPercentage < 0 || importTaxPercentage > 100)
            return Result.Failure(ImportServiceConfigError.InvalidImportTaxPercentage());

        BaseShippingFee = baseShippingFee;
        CountryCode = countryCode;
        CountryName = countryName;
        ImportTaxPercentage = importTaxPercentage;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
