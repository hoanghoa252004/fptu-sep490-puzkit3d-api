using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.DeleteInstockProductPriceDetail;

public sealed record DeleteInstockProductPriceDetailCommand(Guid PriceDetailId) : ICommand;
