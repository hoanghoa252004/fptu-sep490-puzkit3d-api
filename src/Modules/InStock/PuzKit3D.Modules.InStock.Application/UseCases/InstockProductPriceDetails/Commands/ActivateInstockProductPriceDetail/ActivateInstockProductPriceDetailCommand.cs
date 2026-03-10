using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.ActivateInstockProductPriceDetail;

public sealed record ActivateInstockProductPriceDetailCommand(Guid PriceDetailId) : ICommand;
