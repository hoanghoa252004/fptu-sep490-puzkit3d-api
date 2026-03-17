using Microsoft.Extensions.Options;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Options;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;
using System.Text;

namespace PuzKit3D.Modules.Delivery.Infrastructure.Services;

public sealed class GhnDeliveryService : IDeliveryService
{
    private readonly HttpClient _httpClient;
    private readonly DeliverySettings _settings;

    public GhnDeliveryService(HttpClient httpClient, IOptions<DeliverySettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task<ResultT<object>> GetProvincesAsync()
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/master-data/province";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("token", _settings.GhnApiKey.ApiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<object>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<object>(content);

            return Result.Success(jsonData ?? new object());
        }
        catch (Exception ex)
        {
            return Result.Failure<object>(
                Error.Failure("GHN_EXCEPTION", $"Error fetching provinces: {ex.Message}"));
        }
    }

    public async Task<ResultT<object>> GetDistrictsByProvinceAsync(int provinceId)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/master-data/district";
            
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("token", _settings.GhnApiKey.ApiKey);

            var requestBody = JsonSerializer.Serialize(new { province_id = provinceId });
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<object>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<object>(content);

            return Result.Success(jsonData ?? new object());
        }
        catch (Exception ex)
        {
            return Result.Failure<object>(
                Error.Failure("GHN_EXCEPTION", $"Error fetching districts: {ex.Message}"));
        }
    }

    public async Task<ResultT<object>> GetWardsByDistrictAsync(int districtId)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/master-data/ward?district_id={districtId}";
            
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("token", _settings.GhnApiKey.ApiKey);

            var requestBody = JsonSerializer.Serialize(new { district_id = districtId });
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<object>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<object>(content);

            return Result.Success(jsonData ?? new object());
        }
        catch (Exception ex)
        {
            return Result.Failure<object>(
                Error.Failure("GHN_EXCEPTION", $"Error fetching wards: {ex.Message}"));
        }
    }

    public async Task<ResultT<object>> CalculateShippingFeeAsync(object request)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/v2/shipping-order/fee";
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("token", _settings.GhnApiKey.ApiKey);
            httpRequest.Headers.Add("ShopId", _settings.MyShop.ShopId);

            var requestBody = JsonSerializer.Serialize(request);
            httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<object>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<object>(content);

            return Result.Success(jsonData ?? new object());
        }
        catch (Exception ex)
        {
            return Result.Failure<object>(
                Error.Failure("GHN_EXCEPTION", $"Error calculating shipping fee: {ex.Message}"));
        }
    }

    public async Task<ResultT<object>> GetAvailableServicesAsync(int fromDistrict, int toDistrict)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/v2/shipping-order/available-services";
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("token", _settings.GhnApiKey.ApiKey);

            var requestData = new
            {
                shop_id = int.Parse(_settings.MyShop.ShopId),
                from_district = fromDistrict,
                to_district = toDistrict
            };

            var requestBody = JsonSerializer.Serialize(requestData);
            httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<object>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<object>(content);

            return Result.Success(jsonData ?? new object());
        }
        catch (Exception ex)
        {
            return Result.Failure<object>(
                Error.Failure("GHN_EXCEPTION", $"Error fetching available services: {ex.Message}"));
        }
    }

    public async Task<ResultT<object>> CreateShippingOrderAsync(object request)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/v2/shipping-order/create";
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("token", _settings.GhnApiKey.ApiKey);
            httpRequest.Headers.Add("ShopId", _settings.MyShop.ShopId);

            var requestBody = JsonSerializer.Serialize(request);
            httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<object>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<object>(content);

            return Result.Success(jsonData ?? new object());
        }
        catch (Exception ex)
        {
            return Result.Failure<object>(
                Error.Failure("GHN_EXCEPTION", $"Error creating shipping order: {ex.Message}"));
        }
    }
}
