using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Services;

public interface IDeliveryService
{
    /// <summary>
    /// Get all provinces from GHN
    /// </summary>
    /// <returns>Raw JSON response from GHN</returns>
    Task<ResultT<object>> GetProvincesAsync();

    /// <summary>
    /// Get districts by province ID from GHN
    /// </summary>
    /// <param name="provinceId">Province ID from GHN</param>
    /// <returns>Raw JSON response from GHN</returns>
    Task<ResultT<object>> GetDistrictsByProvinceAsync(int provinceId);

    /// <summary>
    /// Get wards by district ID from GHN
    /// </summary>
    /// <param name="districtId">District ID from GHN</param>
    /// <returns>Raw JSON response from GHN</returns>
    Task<ResultT<object>> GetWardsByDistrictAsync(int districtId);
}
