namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.UpdateInstockProduct;

public sealed record UpdateDriveDetailDto(
    Guid DriveId,
    int Quantity);
