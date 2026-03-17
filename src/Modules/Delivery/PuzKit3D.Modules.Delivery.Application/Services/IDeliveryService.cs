using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Services;

public interface IDeliveryService
{
    Task<ResultT<object>> GetProvincesAsync();
    Task<ResultT<object>> GetDistrictsByProvinceAsync(int provinceId);
    Task<ResultT<object>> GetWardsByDistrictAsync(int districtId);
    Task<ResultT<object>> CalculateShippingFeeAsync(object request);
    Task<ResultT<object>> GetAvailableServicesAsync(int fromDistrict, int toDistrict);
    Task<ResultT<object>> CreateShippingOrderAsync(object request);
    Task<ResultT<object>> GetShippingOrderDetailAsync(string orderCode);
}
