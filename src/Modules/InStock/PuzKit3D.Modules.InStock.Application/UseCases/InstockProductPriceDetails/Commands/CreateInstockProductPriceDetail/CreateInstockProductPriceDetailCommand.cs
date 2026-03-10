using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.CreateInstockProductPriceDetail;

public sealed record CreateInstockProductPriceDetailCommand(
    Guid PriceId,
    Guid VariantId,
    decimal UnitPrice,
    bool IsActive = false) : ICommandT<Guid>;
