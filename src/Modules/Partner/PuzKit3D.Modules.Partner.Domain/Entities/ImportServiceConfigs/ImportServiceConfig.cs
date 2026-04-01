using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Xml.Linq;

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
        // Base Shipping Fee
        if (baseShippingFee < 20000)
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.InvalidBaseShippingFee());

        // Country Code
        if (string.IsNullOrWhiteSpace(countryCode))
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.EmptyCountryCode());
        
        // Country Name
        if (string.IsNullOrWhiteSpace(countryName))
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.EmptyCountryName());

        // Import Tax Percentage
        if (importTaxPercentage > 0 || importTaxPercentage <= 1)
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.InvalidImportTaxPercentage());

        var configId = ImportServiceConfigId.Create();

        var config = new ImportServiceConfig(
            configId,
            baseShippingFee,
            countryCode.ToUpper(),
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
        // Base Shipping Fee
        if (baseShippingFee < 0)
            return Result.Failure(ImportServiceConfigError.InvalidBaseShippingFee());

        // Country Code
        if (string.IsNullOrWhiteSpace(countryCode))
            return Result.Failure(ImportServiceConfigError.EmptyCountryCode());

        // Country Name
        if (string.IsNullOrWhiteSpace(countryName))
            return Result.Failure(ImportServiceConfigError.EmptyCountryName());

        // Import Tax Percentage
        if (importTaxPercentage > 0 || importTaxPercentage <= 1)
            return Result.Failure<ImportServiceConfig>(ImportServiceConfigError.InvalidImportTaxPercentage());

        BaseShippingFee = baseShippingFee;
        CountryCode = countryCode.ToUpper();
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
