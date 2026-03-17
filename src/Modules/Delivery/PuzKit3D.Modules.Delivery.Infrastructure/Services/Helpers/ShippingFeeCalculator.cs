using System.Text.Json;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Infrastructure.Services.Helpers;

public class ShippingFeeCalculator
{
    private readonly IDeliveryService _deliveryService;

    public ShippingFeeCalculator(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    public async Task<ResultT<int>> CalculateAsync(CalculateShippingFeeByLocationRequest request, int fromDistrictId)
    {
        try
        {
            // Step 1: Get province ID from province name
            var provincesResult = await _deliveryService.GetProvincesAsync();
            if (!provincesResult.IsSuccess)
                return Result.Failure<int>(provincesResult.Error);

            var provinceId = ExtractProvinceId(provincesResult.Value, request.ProvinceName);
            if (provinceId == 0)
                return Result.Failure<int>(
                    Error.Failure("NOT_FOUND", $"Province '{request.ProvinceName}' not found"));

            // Step 2: Get district ID from district name
            var districtsResult = await _deliveryService.GetDistrictsByProvinceAsync(provinceId);
            if (!districtsResult.IsSuccess)
                return Result.Failure<int>(districtsResult.Error);

            var districtId = ExtractDistrictId(districtsResult.Value, request.DistrictName);
            if (districtId == 0)
                return Result.Failure<int>(
                    Error.Failure("NOT_FOUND", $"District '{request.DistrictName}' not found"));

            // Step 3: Get ward code from ward name
            var wardsResult = await _deliveryService.GetWardsByDistrictAsync(districtId);
            if (!wardsResult.IsSuccess)
                return Result.Failure<int>(wardsResult.Error);

            var wardCode = ExtractWardCode(wardsResult.Value, request.WardName);
            if (string.IsNullOrEmpty(wardCode))
                return Result.Failure<int>(
                    Error.Failure("NOT_FOUND", $"Ward '{request.WardName}' not found"));

            // Step 4: Get available services
            var servicesResult = await _deliveryService.GetAvailableServicesAsync(fromDistrictId, districtId);
            if (!servicesResult.IsSuccess)
                return Result.Failure<int>(servicesResult.Error);

            var (serviceId, serviceTypeId) = ExtractServiceIds(servicesResult.Value);
            if (serviceId == 0)
                return Result.Failure<int>(
                    Error.Failure("NOT_FOUND", "No service with service_type_id = 2 found"));

            // Step 5: Calculate fee (weight = 1 always)
            var feeRequest = new
            {
                to_district_id = districtId,
                to_ward_code = wardCode,
                weight = 1,
                service_id = serviceId,
                service_type_id = serviceTypeId
            };

            var feeResult = await _deliveryService.CalculateShippingFeeAsync(feeRequest);
            if (!feeResult.IsSuccess)
                return Result.Failure<int>(feeResult.Error);

            var total = ExtractTotal(feeResult.Value);
            return Result.Success(total);
        }
        catch (Exception ex)
        {
            return Result.Failure<int>(
                Error.Failure("CALCULATION_ERROR", $"Error calculating shipping fee: {ex.Message}"));
        }
    }

    private int ExtractProvinceId(object provincesData, string provinceName)
    {
        try
        {
            var json = JsonSerializer.Serialize(provincesData);
            using var doc = JsonDocument.Parse(json);

            var provinces = doc.RootElement.GetProperty("data");
            foreach (var province in provinces.EnumerateArray())
            {
                var provName = province.GetProperty("ProvinceName").GetString();
                if (provName?.Contains(provinceName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return province.GetProperty("ProvinceID").GetInt32();
                }

                // Check NameExtension
                if (province.TryGetProperty("NameExtension", out var extensions))
                {
                    foreach (var ext in extensions.EnumerateArray())
                    {
                        if (ext.GetString()?.Contains(provinceName, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            return province.GetProperty("ProvinceID").GetInt32();
                        }
                    }
                }
            }
        }
        catch { }
        return 0;
    }

    private int ExtractDistrictId(object districtsData, string districtName)
    {
        try
        {
            var json = JsonSerializer.Serialize(districtsData);
            using var doc = JsonDocument.Parse(json);

            var districts = doc.RootElement.GetProperty("data");
            foreach (var district in districts.EnumerateArray())
            {
                var distName = district.GetProperty("DistrictName").GetString();
                if (distName?.Contains(districtName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return district.GetProperty("DistrictID").GetInt32();
                }

                // Check NameExtension
                if (district.TryGetProperty("NameExtension", out var extensions))
                {
                    foreach (var ext in extensions.EnumerateArray())
                    {
                        if (ext.GetString()?.Contains(districtName, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            return district.GetProperty("DistrictID").GetInt32();
                        }
                    }
                }
            }
        }
        catch { }
        return 0;
    }

    private string ExtractWardCode(object wardsData, string wardName)
    {
        try
        {
            var json = JsonSerializer.Serialize(wardsData);
            using var doc = JsonDocument.Parse(json);

            var wards = doc.RootElement.GetProperty("data");
            foreach (var ward in wards.EnumerateArray())
            {
                var wName = ward.GetProperty("WardName").GetString();
                if (wName?.Contains(wardName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return ward.GetProperty("WardCode").GetString() ?? string.Empty;
                }

                // Check NameExtension
                if (ward.TryGetProperty("NameExtension", out var extensions))
                {
                    foreach (var ext in extensions.EnumerateArray())
                    {
                        if (ext.GetString()?.Contains(wardName, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            return ward.GetProperty("WardCode").GetString() ?? string.Empty;
                        }
                    }
                }
            }
        }
        catch { }
        return string.Empty;
    }

    private (int serviceId, int serviceTypeId) ExtractServiceIds(object servicesData)
    {
        try
        {
            var json = JsonSerializer.Serialize(servicesData);
            using var doc = JsonDocument.Parse(json);

            var services = doc.RootElement.GetProperty("data");
            foreach (var service in services.EnumerateArray())
            {
                var serviceTypeId = service.GetProperty("service_type_id").GetInt32();
                if (serviceTypeId == 2)
                {
                    var serviceId = service.GetProperty("service_id").GetInt32();
                    return (serviceId, serviceTypeId);
                }
            }
        }
        catch { }
        return (0, 0);
    }

    private int ExtractTotal(object feeData)
    {
        try
        {
            var json = JsonSerializer.Serialize(feeData);
            using var doc = JsonDocument.Parse(json);

            var data = doc.RootElement.GetProperty("data");
            return data.GetProperty("total").GetInt32();
        }
        catch
        {
            return 0;
        }
    }
}
