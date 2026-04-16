using Microsoft.Extensions.Options;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Mappers;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Options;
using PuzKit3D.Modules.Delivery.Infrastructure.Services.Helpers;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text;
using System.Text.Json;

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

    public async Task<ResultT<object>> CreateShippingOrderAsync(CreateShippingOrderRequest request, FromDto fromtDto, ToDto toDto, DeliveryTrackingType trackingType, CancellationToken cancellationToken = default)
    {
        try
        {
            var senderInfo = new SenderInfo
            {
                Name = fromtDto.FullName,
                Phone = fromtDto.PhoneNumber,
                Address = fromtDto.StreetAddress,
                Ward = fromtDto.Ward,
                District = fromtDto.District,
                Province = fromtDto.Province
            };

            var ghnRequest = request.ToGhnRequest(senderInfo, trackingType);
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/v2/shipping-order/create";
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("token", _settings.GhnApiKey.ApiKey);
            httpRequest.Headers.Add("ShopId", _settings.MyShop.ShopId);

            var requestBody = JsonSerializer.Serialize(ghnRequest);
            httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

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

    public async Task<ResultT<object>> GetShippingOrderDetailAsync(string orderCode)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/v2/shipping-order/detail";
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("token", _settings.GhnApiKey.ApiKey);

            var requestData = new { order_code = orderCode };
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
                Error.Failure("GHN_EXCEPTION", $"Error getting shipping order detail: {ex.Message}"));
        }
    }

    public async Task<ResultT<string>> GeneratePrintTokenAsync(List<string> orderCodes)
    {
        try
        {
            var url = $"{_settings.GhnApiKey.ApiEndpoint}/v2/a5/gen-token";
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("token", _settings.GhnApiKey.ApiKey);

            var requestData = new { order_codes = orderCodes };
            var requestBody = JsonSerializer.Serialize(requestData);
            httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"GHN API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                return Result.Failure<string>(
                    Error.Failure("GHN_API_ERROR", errorMessage));
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var token = doc.RootElement.GetProperty("data").GetProperty("token").GetString();

            if (string.IsNullOrEmpty(token))
            {
                return Result.Failure<string>(
                    Error.Failure("GHN_API_ERROR", "Failed to generate print token"));
            }

            return Result.Success(token);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(
                Error.Failure("GHN_EXCEPTION", $"Error generating print token: {ex.Message}"));
        }
    }

    public async Task<ResultT<string>> GetPrintOrderUrlAsync(string token)
    {
        try
        {
            var url = $"https://dev-online-gateway.ghn.vn/a5/public-api/printA5?token={token}";
            return Result.Success(url);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(
                Error.Failure("GHN_EXCEPTION", $"Error generating print URL: {ex.Message}"));
        }
    }

    public async Task<ResultT<string>> PrintOrderAsync(string token, string format)
    {
        return await GetPrintOrderUrlAsync(token);
    }

    public async Task<ResultT<int>> CalculateShippingFeeByLocationAsync(CalculateShippingFeeByLocationRequest request)
    {
        var fromDistrictId = int.Parse(_settings.MyShop.DistrictId ?? "1");
        var calculator = new ShippingFeeCalculator(this);
        return await calculator.CalculateAsync(request, fromDistrictId);
    }
}
