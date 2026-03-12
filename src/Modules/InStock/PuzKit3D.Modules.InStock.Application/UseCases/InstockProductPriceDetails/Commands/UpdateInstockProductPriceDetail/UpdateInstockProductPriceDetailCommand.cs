using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.UpdateInstockProductPriceDetail;

public sealed record UpdateInstockProductPriceDetailCommand(
Guid PriceDetailId,
decimal? UnitPrice = null,
bool? IsActive = null) : ICommand;
