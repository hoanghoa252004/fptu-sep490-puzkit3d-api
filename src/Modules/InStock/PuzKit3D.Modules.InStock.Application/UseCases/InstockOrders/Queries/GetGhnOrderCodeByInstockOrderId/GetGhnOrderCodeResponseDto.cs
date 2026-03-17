namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetGhnOrderCodeByInstockOrderId;

public sealed record GetGhnOrderCodeResponseDto(
    string OrderCode,
    string SortCode,
    DateTime ExpectedDeliveryTime);
